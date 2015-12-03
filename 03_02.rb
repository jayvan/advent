#! /usr/bin/env ruby

visits = {}
locations = [[0, 0], [0, 0]]
index = 0

ARGF.each_line do |line|
  line.chars.each do |direction|
    if direction == '>'
      locations[index][0] += 1
    elsif direction == '<'
      locations[index][0] -= 1
    elsif direction == '^'
      locations[index][1] += 1
    elsif direction == 'v'
      locations[index][1] -= 1
    end
    visits[[locations[index][0],locations[index][1]]] = true

    index = index + 1
    index %= locations.length
  end

end

puts visits.keys.length
