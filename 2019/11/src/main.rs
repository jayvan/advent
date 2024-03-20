use std::cell::RefCell;
use std::cmp::{max, min};
use std::collections::HashMap;
use std::error::Error;
use std::io::{stdin, Read};
use std::ops::Deref;

struct Intcode<I, O>
where
    I: Fn() -> isize,
    O: FnMut(isize),
{
    opcodes: HashMap<usize, isize>,
    pc: usize,
    rbase: isize,
    input_fn: I,
    output_fn: O,
}

impl<I, O> Intcode<I, O>
where
    I: Fn() -> isize,
    O: FnMut(isize),
{
    pub fn new(program: Vec<isize>, input_fn: I, output_fn: O) -> Intcode<I, O> {
        let mut opcodes = HashMap::new();

        for (i, opcode) in program.iter().enumerate() {
            opcodes.insert(i, *opcode);
        }

        Intcode {
            opcodes,
            pc: 0,
            rbase: 0,
            input_fn,
            output_fn,
        }
    }

    fn get_value(&mut self, address_mode: usize, address: isize) -> isize {
        match address_mode {
            0 => self.read(address as usize),
            1 => address,
            2 => self.read((address + self.rbase) as usize),
            _ => panic!(
                "Found invalid address mode at location {address}: {}",
                address_mode
            ),
        }
    }

    fn get_param(&mut self, num: usize) -> isize {
        let address_mode =
            ((self.cur_instruction() / (10_isize.pow((num as u32) + 2))) % 10) as usize;
        let val = self.get_value(address_mode, self.opcodes[&(self.pc + num + 1)]);
        val
    }

    fn cur_instruction(&self) -> isize {
        self.opcodes[&self.pc]
    }

    fn write(&mut self, index: usize, value: isize) {
        self.opcodes.insert(index, value);
    }

    fn read(&mut self, index: usize) -> isize {
        *self.opcodes.entry(index).or_default()
    }

    pub fn write_location(&self, num: u32) -> usize {
        match self.opcodes[&self.pc] / 10_isize.pow(num + 1) {
            0 => self.opcodes[&(self.pc + num as usize)] as usize,
            2 => (self.opcodes[&(self.pc + num as usize)] + self.rbase) as usize,
            _ => panic!("Invalid offset for read input"),
        }
    }

    pub fn run(&mut self) {
        loop {
            match self.opcodes[&self.pc] % 100 {
                1 => {
                    let new_val = self.get_param(0) + self.get_param(1);
                    let write_location = self.write_location(3);
                    self.write(write_location, new_val);
                    self.pc += 4;
                }
                2 => {
                    let new_val = self.get_param(0) * self.get_param(1);
                    let write_location = self.write_location(3);
                    self.write(write_location, new_val);
                    self.pc += 4;
                }
                3 => {
                    let write_location = self.write_location(1);
                    let input = (self.input_fn)();
                    self.write(write_location, input);
                    self.pc += 2;
                }
                4 => {
                    let output_value = self.get_param(0);
                    (self.output_fn)(output_value);
                    self.pc += 2;
                }
                5 => {
                    let value = self.get_param(0);

                    if value != 0 {
                        self.pc = self.get_param(1) as usize;
                    } else {
                        self.pc += 3;
                    }
                }
                6 => {
                    let value = self.get_param(0);

                    if value == 0 {
                        self.pc = self.get_param(1) as usize;
                    } else {
                        self.pc += 3;
                    }
                }
                7 => {
                    let a = self.get_param(0);
                    let b = self.get_param(1);

                    let write_location = self.write_location(3);
                    self.write(write_location, if a < b { 1 } else { 0 });
                    self.pc += 4;
                }
                8 => {
                    let a = self.get_param(0);
                    let b = self.get_param(1);

                    let write_location = self.write_location(3);
                    self.write(write_location, if a == b { 1 } else { 0 });
                    self.pc += 4;
                }
                9 => {
                    let delta = self.get_param(0);
                    self.rbase += delta;
                    self.pc += 2;
                }
                99 => return,
                _ => panic!(
                    "Found invalid opcode at location {}: {}",
                    self.pc, self.opcodes[&self.pc]
                ),
            }
        }
    }
}

fn main() -> Result<(), Box<dyn Error>> {
    let mut vec_buf = Vec::new();
    stdin().read_to_end(&mut vec_buf)?;
    let mut buf = String::new();
    for char in vec_buf {
        if !char.is_ascii_whitespace() {
            buf.push(char as char);
        }
    }
    let opcodes: Vec<isize> = buf
        .trim()
        .split(",")
        .map(|num| num.parse::<isize>().unwrap())
        .collect();

    let location: RefCell<(i32, i32)> = RefCell::new((0, 0));
    let mut direction: isize = 0;
    let direction_deltas = [(0, 1), (1, 0), (0, -1), (-1, 0)];
    let painting: RefCell<HashMap<(i32, i32), bool>> = RefCell::new(HashMap::new());
    let mut is_painting = true;

    let handle_paint = |num| {
        painting.borrow_mut().insert(*location.borrow(), num == 1);
    };

    let mut handle_turn = |num| {
        direction = (if num == 0 {
            direction - 1
        } else {
            direction + 1
        })
        .rem_euclid(4);
        let delta = &direction_deltas[direction as usize];
        let mut location_mut = location.borrow_mut();
        location_mut.0 += delta.0;
        location_mut.1 += delta.1;
    };

    let handle_output = |num| {
        if is_painting {
            handle_paint(num)
        } else {
            handle_turn(num)
        }
        is_painting = !is_painting;
    };

    let handle_input = || -> isize {
        let borrowed_location = location.borrow();
        if *painting
            .borrow()
            .get(borrowed_location.deref())
            .unwrap_or(&false)
        {
            1
        } else {
            0
        }
    };

    painting.borrow_mut().insert((0, 0), true);

    let mut program = Intcode::new(opcodes, handle_input, handle_output);
    program.run();

    let mut lower = (i32::MAX, i32::MAX);
    let mut upper = (i32::MIN, i32::MIN);

    for location in painting.borrow().keys() {
        lower.0 = min(lower.0, location.0);
        lower.1 = min(lower.1, location.1);

        upper.0 = max(upper.0, location.0);
        upper.1 = max(upper.1, location.1);
    }

    println!("{:?} to {:?}", lower, upper);

    for y in (lower.1..=upper.1).rev() {
        for x in lower.0..=upper.0 {
            let painted = *painting.borrow().get(&(x, y)).unwrap_or(&false);
            print!("{}", if painted { 'X' } else { ' ' });
        }
        print!("\n");
    }

    Ok(())
}
