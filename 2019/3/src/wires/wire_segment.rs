use std::str::FromStr;

#[derive(PartialEq, Debug)]
pub enum Direction { R, U, D, L }

#[derive(PartialEq, Debug)]
pub struct WireSegment {
    direction: Direction,
    length: u32
}

impl WireSegment {
    pub fn direction(&self) -> &Direction {
        &self.direction
    }

    pub fn length(&self) -> u32 {
        self.length
    }
}

impl FromStr for WireSegment {
    type Err = String;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let direction = match s.get(0..1) {
            Some("R") => Direction::R,
            Some("U") => Direction::U,
            Some("L") => Direction::L,
            Some("D") => Direction::D,
            Some(a) => return Err(format!("Invalid direction {:?}", a)),
            None => return Err(String::from("Missing direction"))
        };

        if let Ok(length) = s[1..].parse::<u32>() {
            Ok(WireSegment { direction, length })
        } else {
            Err("Invalid length".to_string())
        }
    }
}
#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn wire_segment_parsing() {
        // R75,D30,R83,U83,L12
        let a = "R75".parse::<WireSegment>().unwrap();
        assert_eq!(a, WireSegment{ direction: Direction::R, length: 75 });

        let b = "D30".parse::<WireSegment>().unwrap();
        assert_eq!(b, WireSegment{ direction: Direction::D, length: 30 });

        let c = "U83".parse::<WireSegment>().unwrap();
        assert_eq!(c, WireSegment{ direction: Direction::U, length: 83 });

        let d = "L12".parse::<WireSegment>().unwrap();
        assert_eq!(d, WireSegment{ direction: Direction::L, length: 12 });
    }

    #[test]
    fn wire_segment_missing_direction() {
        assert!("65".parse::<WireSegment>().is_err());
    }

    #[test]
    fn wire_segment_invalid_direction() {
        assert!("Z75".parse::<WireSegment>().is_err());
    }

    #[test]
    fn wire_segment_missing_length() {
        assert!("L".parse::<WireSegment>().is_err());
    }
}
