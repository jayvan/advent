#!/usr/bin/env ruby

row = 2981
column = 3075

base_row = row + column - 1
number = 1.upto(base_row - 1).inject(:+) + column - 1

current = 20151125

number.times do
  current = (current * 252533) % 33554393
end

puts current
