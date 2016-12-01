#! /usr/bin/env ruby

def search(graph, current = nil, visited = [])
  targets = graph.keys - visited - [nil]
  return 0 if targets.empty?

  targets.map do |target|
    graph[current][target] + search(graph, target, visited + [target])
  end.min
end

graph = Hash.new { |hash, key| hash[key] = Hash.new(0) }
ARGF.each_line do |line|
  a, _, b, _, cost = line.chomp.split(' ')

  graph[a][b] = cost.to_i
  graph[b][a] = cost.to_i
end
puts search(graph)
