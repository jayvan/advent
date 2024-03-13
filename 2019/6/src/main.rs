use std::collections::{HashMap, HashSet, VecDeque};
use std::error::Error;
use std::io::stdin;

struct Graph {
    edges: HashMap<String, HashSet<String>>
}

impl Graph {
    pub fn new() -> Graph {
        Graph { edges: HashMap::new() }
    }
    pub fn add_edge(&mut self, from: String, to: String) {
        self.edges.entry(from.clone()).or_default().insert(to.clone());
        self.edges.entry(to).or_default().insert(from);
    }

    pub fn get_orbit(&self, entity: String) -> &String {
        self.edges.get(&entity).unwrap().iter().nth(0).unwrap()
    }

    pub fn shortest_path(&self, from: &String, to: &String) -> usize {
        let mut q: VecDeque<(&String, HashSet<&String>, usize)> = VecDeque::new();
        let mut visited: HashSet<&String> = HashSet::new();
        visited.insert(from);
        q.push_back((from, visited, 0));

        loop {
            match q.pop_front() {
                None => { return usize::MAX; }
                Some((entity, visited, distance)) => {
                    if entity == to {
                        return distance;
                    }

                    if let Some(neighbors) = self.edges.get(entity) {
                        for adjacent in neighbors {
                            if !visited.contains(adjacent) {
                                let mut new_visited = visited.clone();
                                new_visited.insert(adjacent);
                                q.push_back((adjacent, new_visited, distance + 1));
                            }
                        }
                    }
                }
            }
        }
    }
}

fn main() -> Result<(), Box<dyn Error>> {
    let mut graph = Graph::new();

    for line in stdin().lines() {
        let unwrapped_line = line.unwrap();
        let mut entry = unwrapped_line.trim().split(')');
        let Some(to) = entry.next() else { continue };
        let Some(from) = entry.next() else { continue };
        graph.add_edge(from.to_string(), to.to_string());
    }

    let my_orbit = graph.get_orbit("YOU".to_string());
    let santa_orbit = graph.get_orbit("SAN".to_string());
    let shortest_path = graph.shortest_path(my_orbit, santa_orbit);

    println!("Shortest path: {shortest_path}");

    Ok(())
}
