use std::str::FromStr;
use crate::wires::wire_segment::WireSegment;

pub struct Wire {
   pub segments: Vec<WireSegment>
}

impl FromStr for Wire {
    type Err = String;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let segments:Vec<WireSegment> = s.split(",").map(|seg| seg.parse::<WireSegment>().unwrap()).collect::<Vec<WireSegment>>();
        Ok(Wire { segments })
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    use crate::wires::wire_segment::Direction;

    #[test]
    fn wire_parsing() {
        let wire = ("R8,U5,L7,D3").parse::<Wire>().unwrap();
        assert_eq!(4, wire.segments.len());

        assert_eq!(*wire.segments[0].direction(), Direction::R);
        assert_eq!(wire.segments[0].length(), 8);

        assert_eq!(*wire.segments[1].direction(), Direction::U);
        assert_eq!(wire.segments[1].length(), 5);

        assert_eq!(*wire.segments[2].direction(), Direction::L);
        assert_eq!(wire.segments[2].length(), 7);

        assert_eq!(*wire.segments[3].direction(), Direction::D);
        assert_eq!(wire.segments[3].length(), 3);
    }
}
