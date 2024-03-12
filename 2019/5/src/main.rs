use std::error::Error;
use std::io::stdin;

pub fn get_digit(num: isize, digit: u32) -> usize {
    ((num / (10_isize.pow(digit))) % 10) as usize
}

fn get_value(address_mode: usize, address: isize, opcodes: &Vec<isize>) -> isize {
    match address_mode {
        0 => opcodes[address as usize],
        1 => address,
        _ => panic!("Found invalid address mode at location {address}: {}", address_mode)
    }
}

fn get_param(opcodes: &Vec<isize>, pc: usize, num: usize) -> isize {
    let address_mode = get_digit(opcodes[pc], (num + 2) as u32);
    get_value(address_mode, opcodes[pc + num + 1], &opcodes)
}

fn run_intcode(template: &Vec<isize>, input: isize) -> isize {
    let mut opcodes = template.clone();

    let mut pc = 0;

    loop {
        match opcodes[pc] % 100 {
            1 => {
                let new_val = get_param(&opcodes, pc, 0) + get_param(&opcodes, pc, 1);
                let write_location = opcodes[pc + 3];
                opcodes[write_location as usize] = new_val;
                pc += 4;
            },
            2 => {
                let new_val = get_param(&opcodes, pc, 0) * get_param(&opcodes, pc, 1);
                let write_location = opcodes[pc + 3];
                opcodes[write_location as usize] = new_val;
                pc += 4;
            },
            3 => {
                let write_location = opcodes[pc + 1];
                opcodes[write_location as usize] = input;
                pc += 2;
            },
            4 => {
                let output_value = get_param(&opcodes, pc, 0);
                println!("{output_value}");
                pc += 2;
            },
            5 => {
                let value = get_param(&opcodes, pc, 0);

                if value != 0 {
                    pc = get_param(&opcodes, pc, 1) as usize;
                } else {
                    pc += 3;
                }
            },
            6 => {
                let value = get_param(&opcodes, pc, 0);

                if value == 0 {
                    pc = get_param(&opcodes, pc, 1) as usize;
                } else {
                    pc += 3;
                }
            },
            7 => {
                let a = get_param(&opcodes, pc, 0);
                let b = get_param(&opcodes, pc, 1);

                let write_location = opcodes[pc + 3];
                opcodes[write_location as usize] = if a < b { 1 } else { 0 };
                pc += 4;
            },
            8 => {
                let a = get_param(&opcodes, pc, 0);
                let b = get_param(&opcodes, pc, 1);

                let write_location = opcodes[pc + 3];
                opcodes[write_location as usize] = if a == b { 1 } else { 0 };
                pc += 4;
            },
            99 => { return opcodes[0] },
            _ => panic!("Found invalid opcode at location {pc}: {}", opcodes[pc])
        }
    }
}

fn main() -> Result<(), Box<dyn Error>>{
    let mut buf = String::new();
    stdin().read_line(&mut buf)?;
    let opcodes: Vec<isize> = buf.trim().split(",").map(|num| num.parse::<isize>().unwrap()).collect();

    run_intcode(&opcodes, 5);

    Ok(())
}
