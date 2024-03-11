use std::cmp::min;
use std::collections::HashMap;
use std::error::Error;
use std::io::stdin;
use crate::wires::wire::Wire;
use crate::wires::wire_path::WirePath;

pub mod wires;

fn main() -> Result<(), Box<dyn Error>> {
    let mut buf_a = String::new();
    let mut buf_b = String::new();
    stdin().read_line(&mut buf_a)?;
    stdin().read_line(&mut buf_b)?;

    let wire_a = buf_a.trim().parse::<Wire>()?;
    let wire_b = buf_b.trim().parse::<Wire>()?;

    let mut wire_a_points = HashMap::new();

    for (i, point) in WirePath::from_wire(&wire_a).enumerate() {
        wire_a_points.entry(point).or_insert(i + 1);
    }

    let mut least_steps = usize::MAX;

    for (i, point) in WirePath::from_wire(&wire_b).enumerate() {
        if wire_a_points.contains_key(&point) {
            least_steps = min(least_steps, i + 1 + wire_a_points[&point]);
        }
    }

    println!("{least_steps}");

    Ok(())
}
