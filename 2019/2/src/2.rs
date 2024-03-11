use std::error::Error;
use std::io::stdin;

fn run_intcode(template: &Vec<usize>, noun: usize, verb: usize) -> usize {
    let mut opcodes = template.clone();

    opcodes[1] = noun;
    opcodes[2] = verb;

    let mut pc = 0;

    loop {
        match opcodes[pc] {
            1 => {
                let new_val = opcodes[opcodes[pc + 1]] + opcodes[opcodes[pc + 2]];
                let write_location = opcodes[pc + 3];
                opcodes[write_location] = new_val;
            },
            2 => {
                let new_val = opcodes[opcodes[pc + 1]] * opcodes[opcodes[pc + 2]];
                let write_location = opcodes[pc + 3];
                opcodes[write_location] = new_val;
            },
            99 => { return opcodes[0] },
            _ => panic!("Found invalid opcode at location {pc}: {}", opcodes[pc])
        }

        pc += 4;
    }
}

fn main() -> Result<(), Box<dyn Error>>{
    let mut buf = String::new();
    stdin().read_line(&mut buf)?;
    let opcodes: Vec<usize> = buf.trim().split(",").map(|num| num.parse::<usize>().unwrap()).collect();

    for noun in 1..=99 {
        for verb in 1..=99 {
            match run_intcode(&opcodes, noun, verb) {
                19690720 => {
                    println!("({noun}, {verb}) -> {}", 100 * noun + verb);
                    break;
                },
                _ => {}
            }
        }
    }

    Ok(())
}
