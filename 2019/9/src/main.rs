use std::collections::{HashMap, VecDeque};
use std::error::Error;
use std::io::{Read, stdin};

struct Intcode {
    opcodes: HashMap<usize, isize>,
    inputs: VecDeque<isize>,
    pc: usize,
    rbase: isize,
    last_output: isize,
}

impl Intcode {
}

impl Intcode {
    pub fn new(program: Vec<isize>) -> Intcode {
        let mut opcodes = HashMap::new();

        for (i, opcode) in program.iter().enumerate() {
            opcodes.insert(i, *opcode);
        }

        Intcode {
            opcodes,
            inputs: VecDeque::new(),
            pc: 0,
            rbase: 0,
            last_output: -1,
        }
    }

    fn get_value(&mut self, address_mode: usize, address: isize) -> isize {
        match address_mode {
            0 => self.read(address as usize),
            1 => address,
            2 => self.read((address + self.rbase) as usize),
            _ => panic!("Found invalid address mode at location {address}: {}", address_mode)
        }
    }

    fn get_param(&mut self, num: usize) -> isize {
        let address_mode = ((self.cur_instruction() / (10_isize.pow((num as u32) + 2))) % 10) as usize;
        let val = self.get_value(address_mode, self.opcodes[&(self.pc + num + 1)]);
        val
    }

    fn cur_instruction(&self) -> isize {
        self.opcodes[&self.pc]
    }

    pub fn add_input(&mut self, input: isize) {
        self.inputs.push_back(input);
    }

    fn write(&mut self, index: usize, value: isize) {
        println!("W: ADDR {index} SET {value}");
        self.opcodes.insert(index, value);
    }

    fn read(&mut self, index: usize) -> isize {
        println!("R: ADDR {index} GOT {}", self.opcodes[&index]);
        *self.opcodes.entry(index).or_default()
    }

    pub fn write_location(&self, num: u32) -> usize {
        match self.opcodes[&self.pc] / 10_isize.pow(num + 1) {
            0 => self.opcodes[&(self.pc + num as usize)] as usize,
            2 => (self.opcodes[&(self.pc + num as usize)] + self.rbase) as usize,
            _ => panic!("Invalid offset for read input")
        }
    }

    pub fn run(&mut self) -> isize {
        loop {
            println!("-----{}-----{}-----",self.pc,self.opcodes[&self.pc]);
            match self.opcodes[&self.pc] % 100 {
                1 => {
                    let new_val = self.get_param(0) + self.get_param(1);
                    let write_location = self.write_location(3);
                    self.write(write_location as usize, new_val);
                    self.pc += 4;
                },
                2 => {
                    let new_val = self.get_param(0) * self.get_param(1);
                    let write_location = self.write_location(3);
                    self.write(write_location as usize, new_val);
                    self.pc += 4;
                },
                3 => {
                    let write_location = self.write_location(1);
                    let input = self.inputs.pop_front().expect("Insufficient inputs");
                    self.write(write_location as usize, input);
                    self.pc += 2;
                }
                4 => {
                    self.last_output = self.get_param(0);
                    println!("OUT: {}", self.last_output);
                    self.pc += 2;
                },
                5 => {
                    let value = self.get_param(0);

                    if value != 0 {
                        self.pc = self.get_param(1) as usize;
                        println!("PC SET TO {}", self.pc);
                    } else {
                        self.pc += 3;
                    }
                },
                6 => {
                    let value = self.get_param(0);

                    if value == 0 {
                        self.pc = self.get_param(1) as usize;
                    } else {
                        self.pc += 3;
                    }
                },
                7 => {
                    let a = self.get_param(0);
                    let b = self.get_param(1);

                    let write_location = self.write_location(3);
                    self.write(write_location as usize, if a < b { 1 } else { 0 });
                    self.pc += 4;
                },
                8 => {
                    let a = self.get_param(0);
                    let b = self.get_param(1);

                    let write_location = self.write_location(3);
                    self.write(write_location as usize, if a == b { 1 } else { 0 });
                    self.pc += 4;
                },
                9 => {
                    let delta = self.get_param(0);
                    println!("RBASE DELTA {delta}");
                    println!("RBASE {} -> {}", self.rbase, self.rbase + delta);
                    self.rbase += delta;
                    self.pc += 2;
                },
                99 => {
                    return self.last_output;
                },
                _ => panic!("Found invalid opcode at location {}: {}", self.pc,  self.opcodes[&self.pc])
            }
        }
    }
}

fn main() -> Result<(), Box<dyn Error>>{
    let mut vec_buf = Vec::new();
    stdin().read_to_end(&mut vec_buf)?;
    let mut buf = String::new();
    for char in vec_buf {
        if !char.is_ascii_whitespace() {
            buf.push(char as char);
        }
    }
    let opcodes: Vec<isize> = buf.trim().split(",").map(|num| num.parse::<isize>().unwrap()).collect();

    let mut program = Intcode::new(opcodes);
    program.add_input(2);
    program.run();

    Ok(())
}
