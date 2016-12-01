#! /usr/bin/env ruby

total_paper = 0
total_ribbon = 0

ARGF.each_line do |line|
  dimensions = line.split('x').map(&:to_i)

  front_area = dimensions[0] * dimensions[1]
  top_area = dimensions[1] * dimensions[2]
  side_area = dimensions[2] * dimensions[0]

  total_paper += front_area * 2 + top_area * 2 + side_area * 2
  total_paper += [front_area, top_area, side_area].min

  front_perimeter = dimensions[0] * 2 + dimensions[1] * 2
  top_perimeter = dimensions[1] * 2 + dimensions[2] * 2
  side_perimeter = dimensions[2] * 2 + dimensions[0] * 2
  volume = dimensions[0] * dimensions[1] * dimensions[2]

  total_ribbon += [front_perimeter, top_perimeter, side_perimeter].min
  total_ribbon += volume
end

puts "Total paper needed: #{total_paper}"
puts "Total ribbon needed: #{total_ribbon}"
