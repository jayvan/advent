use std::collections::{HashMap, HashSet};
use std::error::Error;
use std::hash::Hash;
use std::io::stdin;
use num;
use regex::Regex;

fn simulate_plane(original_positions: &[i16]) -> i32 {
    let mut positions: [i16;4] = [original_positions[0], original_positions[1], original_positions[2], original_positions[3]];
    let mut velocities:[i16;4] = [0;4];
    let mut accelerations:[i16;4] = [0;4];
    let mut iteration:i32 = 0;
    let mut seen:HashSet<([i16;4], [i16;4])> = HashSet::new();
    seen.insert((<[i16; 4]>::try_from(positions).unwrap(), velocities));

    loop {
        accelerations.fill(0);

        // Calculate Accelerations
        for planet in 0..4 {
            for other in (planet + 1)..4 {
                let delta = num::clamp(positions[other] - positions[planet], -1, 1);
                accelerations[planet] += delta;
                accelerations[other] -= delta;
            }
        }

        // Update Velocity & position
        for i in 0..4 {
            velocities[i] += accelerations[i];
            positions[i] += velocities[i];
        }

        iteration += 1;
        if !seen.insert((positions, velocities)) {
            return iteration;
        }
    }
}

fn gcd(x: u64, y: u64) -> u64 {
    let mut x = x;
    let mut y = y;

    while y != 0 {
        let t = y;
        y = x % y;
        x = t;
    }

    x
}

fn lcm(a: u64, b: u64) -> u64 {
    (a * b) / gcd(a, b)
}

fn main() -> Result<(), Box<dyn Error>> {
    let mut buffer = String::new();
    let regex = Regex::new(r"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>").expect("Invalid regex");

    let mut positions:[i16;12] = [0;12];

    for i in 0..4 {
        stdin().read_line(&mut buffer)?;

        if let Some(captures) = regex.captures(&buffer) {
            positions[i] = captures[1].parse().unwrap();
            positions[4 + i] = captures[2].parse().unwrap();
            positions[8 + i] = captures[3].parse().unwrap();
        }

        buffer.clear()
    }

    let x_phase = simulate_plane(&positions[0..4]) as u64;
    let y_phase = simulate_plane(&positions[4..8]) as u64;
    let z_phase = simulate_plane(&positions[8..12]) as u64;
        ;

    let answer = lcm(lcm(x_phase, y_phase), z_phase);
    println!("{x_phase}, {y_phase}, {z_phase} -> {answer}");
    Ok(())
}
