use std::cmp::Ordering;
use std::collections::hash_map::DefaultHasher;
use std::collections::{HashMap, HashSet};
use std::hash::{Hash, Hasher};
use std::io::stdin;
use std::time::SystemTime;
use regex::Regex;

#[derive(Hash,Clone)]
struct Planet {
    position: [i32;3],
    velocity: [i32;3],
}

impl Planet {
    fn new(position: [i32;3]) -> Planet {
        Planet {
            position,
            velocity: [0;3],
        }
    }

    fn energy(&self) -> u32 {
        let potential_energy = self.position[0].abs() + self.position[1].abs() + self.position[2].abs();
        let kinetic_energy = self.velocity[0].abs() + self.velocity[1].abs() + self.velocity[2].abs();
        (potential_energy * kinetic_energy) as u32
    }

    fn update_velocity(&mut self, planets: &Vec<[i32;3]>) {
        for planet in planets {
            for i in 0..3 {
                match planet[i].cmp(&self.position[i]) {
                    Ordering::Less => { self.velocity[i] -= 1 }
                    Ordering::Equal => { }
                    Ordering::Greater => { self.velocity[i] += 1}
                }
            }
        }
    }

    fn update_position(&mut self) {
        for i in 0..self.position.len() {
            self.position[i] += self.velocity[i];
        }
    }
}

fn main() {
    let mut buffer = String::new();
    let regex = Regex::new(r"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>").expect("Invalid regex");
    let mut planets:Vec<Planet> = Vec::new();
    let mut seen:HashSet<u64> = HashSet::new();

    while stdin().read_line(&mut buffer).is_ok_and(|size| size > 0) {
        if let Some(captures) = regex.captures(&buffer) {
            let position: [i32;3] = [captures[1].parse().unwrap(), captures[2].parse().unwrap(), captures[3].parse().unwrap()];
            planets.push(Planet::new(position));
        }

        buffer.clear()
    }

    let mut iteration = 0;
    let mut time = SystemTime::now();
    loop {
        let positions:Vec<[i32;3]> = planets.iter().map(|planet| planet.position).collect();
        planets.iter_mut().for_each(|p| p.update_velocity(&positions));
        planets.iter_mut().for_each(|p| p.update_position());

        let mut hasher = DefaultHasher::new();
        planets.hash(&mut hasher);
        let is_new = seen.insert(hasher.finish());

        if !is_new {
            println!("Finished after {iteration} steps");
            break;
        }

        if iteration % 1000000 == 0 {
            println!("{iteration}. {}", SystemTime::now().duration_since(time).unwrap().as_millis());
            time = SystemTime::now();
        }

        iteration += 1;
    }
}
