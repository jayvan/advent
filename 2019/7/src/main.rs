use std::cmp::max;
use std::collections::VecDeque;
use std::error::Error;
use std::io::stdin;

struct Intcode {
    opcodes: Vec<isize>,
    inputs: VecDeque<isize>,
    pc: usize,
    last_output: isize,
    finished: bool
}

impl Intcode {
}

impl Intcode {
    pub fn new(opcodes: Vec<isize>) -> Intcode {
        Intcode {
            opcodes,
            inputs: VecDeque::new(),
            pc: 0,
            last_output: -1,
            finished: false
        }
    }

    pub fn finished(&self) -> bool {
        self.finished
    }

    fn get_value(&self, address_mode: usize, address: isize) -> isize {
        match address_mode {
            0 => self.opcodes[address as usize],
            1 => address,
            _ => panic!("Found invalid address mode at location {address}: {}", address_mode)
        }
    }

    fn get_param(&self, num: usize) -> isize {
        let address_mode = ((self.cur_instruction() / (10_isize.pow((num as u32) + 2))) % 10) as usize;
        self.get_value(address_mode, self.opcodes[self.pc + num + 1])
    }

    fn cur_instruction(&self) -> isize {
        self.opcodes[self.pc]
    }

    pub fn add_input(&mut self, input: isize) {
        self.inputs.push_back(input);
    }

    pub fn run(&mut self) -> isize {
        loop {
            match self.opcodes[self.pc] % 100 {
                1 => {
                    let new_val = self.get_param(0) + self.get_param(1);
                    let write_location = self.opcodes[self.pc + 3];
                    self.opcodes[write_location as usize] = new_val;
                    self.pc += 4;
                },
                2 => {
                    let new_val = self.get_param(0) * self.get_param(1);
                    let write_location = self.opcodes[self.pc + 3];
                    self.opcodes[write_location as usize] = new_val;
                    self.pc += 4;
                },
                3 => {
                    let write_location = self.opcodes[self.pc + 1];
                    self.opcodes[write_location as usize] = self.inputs.pop_front().expect("Insufficient inputs");
                    self.pc += 2;
                },
                4 => {
                    self.last_output = self.get_param(0);
                    self.pc += 2;
                    return self.last_output;
                },
                5 => {
                    let value = self.get_param(0);

                    if value != 0 {
                        self.pc = self.get_param(1) as usize;
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

                    let write_location = self.opcodes[self.pc + 3];
                    self.opcodes[write_location as usize] = if a < b { 1 } else { 0 };
                    self.pc += 4;
                },
                8 => {
                    let a = self.get_param(0);
                    let b = self.get_param(1);

                    let write_location = self.opcodes[self.pc + 3];
                    self.opcodes[write_location as usize] = if a == b { 1 } else { 0 };
                    self.pc += 4;
                },
                99 => {
                    self.finished = true;
                    return self.last_output;
                },
                _ => panic!("Found invalid opcode at location {}: {}", self.pc,  self.opcodes[self.pc])
            }
        }
    }
}




fn permutations(results: &mut Vec<Vec<usize>>, items: &[usize], partial: Vec<usize>) {
    for val in items {
        if !partial.contains(val) {
            let mut new_partial = partial.clone();
            new_partial.push(*val);

            if new_partial.len() == items.len() {
                results.push(new_partial);
            } else {
                permutations(results, items, new_partial);
            }
        }
    }
}

fn main() -> Result<(), Box<dyn Error>>{
    let mut buf = String::new();
    stdin().read_line(&mut buf)?;
    let opcodes: Vec<isize> = buf.trim().split(",").map(|num| num.parse::<isize>().unwrap()).collect();

    let mut perm_buf = Vec::new();
    let foo:[usize;5] = [5,6,7,8,9];
    permutations(&mut perm_buf, &foo, Vec::new());

    let mut highest_output = 0;

    for permutation in perm_buf {
        let mut programs:[Intcode;5] = std::array::from_fn(|_| Intcode::new(opcodes.clone()));
        for (i, program) in programs.iter_mut().enumerate() {
            program.add_input(permutation[i] as isize);
        }

        programs[0].add_input(0);
        let mut program_idx = 0;

        loop {
            let output = programs[program_idx].run();
            if programs[program_idx].finished() {
                println!("{:?} -> {}", permutation, programs[4].last_output);
                highest_output = max(highest_output, programs[4].last_output);
                break;
            }
            program_idx = (program_idx + 1) % 5;
            programs[program_idx].add_input(output);
        }

    }

    println!("{highest_output}");

    Ok(())
}
