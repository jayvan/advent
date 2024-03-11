use std::io::stdin;

fn fuel_cost(mass: i32) -> i32 {
    let base = mass / 3 - 2;
    if base <= 0 { 0 } else { base + fuel_cost(base) }
}

fn main() {
    let mut fuel:i32 = stdin().lines().map(|line| fuel_cost(line.unwrap().parse::<i32>().unwrap())).sum();
    println!("Total fuel: {fuel}");
}
