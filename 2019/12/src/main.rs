// Detect how long until 2 bodies will pass one another and simulate until they do, skipping time

use std::collections::hash_map::DefaultHasher;
use std::collections::{HashMap, HashSet};
use std::hash::{Hash, Hasher};
use std::io::stdin;
use num;
use std::time::SystemTime;
use regex::Regex;

#[derive(Hash,Clone,Debug)]
struct Planet {
    position: [i32;3],
    velocity: [i32;3],
    acceleration: [i32;3],
}

impl Planet {
    fn new(position: [i32;3]) -> Planet {
        Planet {
            position,
            velocity: [0;3],
            acceleration: [0;3],
        }
    }

    fn update_acceleration(&mut self, planets: &Vec<[i32;3]>) {
        self.acceleration[0] = 0;
        self.acceleration[1] = 0;
        self.acceleration[2] = 0;

        for planet in planets {
            for i in 0..3 {
                self.acceleration[i] += num::clamp(planet[i] - self.position[i], -1, 1);
            }
        }
    }

    fn update_velocity(&mut self) {
        self.velocity[0] += self.acceleration[0];
        self.velocity[1] += self.acceleration[1];
        self.velocity[2] += self.acceleration[2];
    }

    fn update_position(&mut self) {
        self.position[0] += self.velocity[0];
        self.position[1] += self.velocity[1];
        self.position[2] += self.velocity[2];
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


    let mut previous_gravity: u64 = 0;
    let mut previous_gravity_tick = iteration;

    loop {
        let positions:Vec<[i32;3]> = planets.iter().map(|planet| planet.position).collect();
        planets.iter_mut().for_each(|p| p.update_acceleration(&positions));
        planets.iter_mut().for_each(|p| p.update_velocity());
        planets.iter_mut().for_each(|p| p.update_position());

        let mut hasher = DefaultHasher::new();
        planets.iter().map(|planet| planet.acceleration).flatten().for_each(|g| hasher.write_i32(g));
        let gravity_hash = hasher.finish();
        if gravity_hash != previous_gravity {
            println!("Gravity change on iteration {iteration}! After {}", iteration - previous_gravity_tick);
            let gravity:Vec<[i32;3]> = planets.iter().map(|p| p.acceleration).collect();
            println!("{:?}", gravity);
            previous_gravity = gravity_hash;
            previous_gravity_tick = iteration;
        }

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
