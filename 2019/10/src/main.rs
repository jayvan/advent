use std::cmp::Ordering;
use std::collections::HashMap;
use std::f64::consts::PI;
use std::io::stdin;

fn get_angle((x1, y1): (usize,usize), (x2, y2): (usize, usize)) -> i32 {
    let result = (-(x1 as f64 - x2 as f64)).atan2(-(y2 as f64 - y1 as f64));
    let degrees = if result < 0.0 { result + PI * 2.0 } else { result } * 360.0 / (PI * 2.0);
    (degrees * 100.0) as i32
}

fn main() {
    // Stations = (x,y) -> (angle, [(x1, y1), (x2, y2) ...])
    let mut stations:HashMap<(usize,usize), HashMap<i32, Vec<(usize,usize)>>> = HashMap::new();
    let mut y:usize = 0;

    let mut buffer = String::new();

    while stdin().read_line(&mut buffer).is_ok_and(|size| size > 0) {
        for (x, char) in buffer.trim().bytes().enumerate() {
            if char != '#' as u8 { continue }

            let current = (x, y);
            let mut angles: HashMap<i32, Vec<(usize,usize)>> = HashMap::new();

            for (other, other_angles) in stations.iter_mut() {
                let other_angle = get_angle(*other, current);
                other_angles.entry(other_angle).or_default().push(current);

                let angle = get_angle(current, *other);
                angles.entry(angle).or_default().push(*other);
            }

            stations.insert(current, angles);
        }

        y += 1;
        buffer.clear();
    }

    let mut best: (usize, usize) = (0,0);
    let mut most: usize = 0;

    for (location, visible) in &stations {
        if visible.len() > most {
            most = visible.len();
            best = *location;
        }
    }

    println!("The best is {:?} with {most}", best);

    // Let's SWEEP!
    let targets = stations.get_mut(&best).expect("");
    let mut angles:Vec<i32> = targets.keys().map(|x| *x).collect();
    angles.sort();

    for inline in targets.values_mut() {
        inline.sort_by(|a, b| distance(*a, best).partial_cmp(&distance(*b, best)).unwrap_or(Ordering::Equal));
    }

    let mut num_shot = 0;
    let mut i = 0;

    loop {
        let angle_targets = targets.get_mut(&angles[i]).unwrap();
        if angle_targets.len() > 0 {
            num_shot += 1;
            if num_shot == 200 {
                let shot_down = angle_targets.pop().unwrap();
                println!("{num_shot}: {:?}. {}", shot_down, shot_down.0 * 100 + shot_down.1);
                break
            }
        }

        i = (i + 1) % angles.len();
    }
}

fn distance((x1, y1): (usize,usize), (x2, y2): (usize, usize)) -> f64 {
    -((y2.abs_diff(y1).pow(2) + x2.abs_diff(x1).pow(2)) as f64).sqrt()
}
