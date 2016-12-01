#! /usr/bin/env ruby

floor = 0
first_basement = nil
ARGF.readline.chars.each_with_index do |char, i|
  if char == '('
    floor += 1
  else
    floor -= 1
    if floor == -1 && first_basement.nil?
      first_basement = i + 1
    end
  end
end

puts "Final floor: #{floor}"
puts "First basement visit: #{first_basement}"
