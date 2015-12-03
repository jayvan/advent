#! /usr/bin/env ruby

visits = {}
x = 0
y = 0

ARGF.each_line do |line|
  line.chars.each do |direction|
    if direction == '>'
      x += 1
    elsif direction == '<'
      x -= 1
    elsif direction == '^'
      y += 1
    elsif direction == 'v'
      y -= 1
    end
    visits[[x, y]] = true
  end
end

puts visits.keys.length
