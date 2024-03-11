use crate::wires::wire::Wire;
use crate::wires::wire_segment::Direction;

#[derive(Debug, Eq, PartialEq, Hash)]
pub struct WirePoint {
    pub x: i32,
    pub y: i32,
}

pub struct WirePath<'a> {
    wire: &'a Wire,
    cur_segment: usize,
    segment_index: u32,
    x: i32,
    y: i32
}

impl<'a> WirePath<'a> {
    pub fn from_wire(wire: &'a Wire) -> WirePath {
        WirePath { wire, cur_segment: 0, segment_index: 0, x: 0, y: 0 }
    }
}

impl<'a> Iterator for WirePath<'a> {
    type Item = WirePoint;

    fn next(&mut self) -> Option<Self::Item> {
        if self.cur_segment >= self.wire.segments.len() {
            return None;
        }

        let segment = &self.wire.segments[self.cur_segment];

        match segment.direction() {
            Direction::R => self.x += 1,
            Direction::U => self.y += 1,
            Direction::D => self.y -= 1,
            Direction::L => self.x -= 1
        }

        self.segment_index += 1;
        if self.segment_index >= segment.length() {
            self.segment_index = 0;
            self.cur_segment += 1;
        }

        Some(WirePoint{ x: self.x, y: self.y })
    }
}

#[cfg(test)]
mod tests {
    use super::*;

   #[test]
   fn test_path() {
       let wire = ("R1,U2,L3,D4").parse::<Wire>().unwrap();
       let mut path = WirePath::from_wire(&wire);

       assert_eq!(path.next(), Some(WirePoint{x: 1, y: 0}));
       assert_eq!(path.next(), Some(WirePoint{x: 1, y: 1}));
       assert_eq!(path.next(), Some(WirePoint{x: 1, y: 2}));
       assert_eq!(path.next(), Some(WirePoint{x: 0, y: 2}));
       assert_eq!(path.next(), Some(WirePoint{x: -1, y: 2}));
       assert_eq!(path.next(), Some(WirePoint{x: -2, y: 2}));
       assert_eq!(path.next(), Some(WirePoint{x: -2, y: 1}));
       assert_eq!(path.next(), Some(WirePoint{x: -2, y: 0}));
       assert_eq!(path.next(), Some(WirePoint{x: -2, y: -1}));
       assert_eq!(path.next(), Some(WirePoint{x: -2, y: -2}));
       assert_eq!(path.next(), None);
   }
}