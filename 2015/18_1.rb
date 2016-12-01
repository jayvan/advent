#! /usr/bin/env ruby

lights = []

ARGF.each_line do |line|
  lights << line.chomp.chars.map { |char| char == '#' }
end

def update(grid)
  new_grid = Array.new(grid.length) { Array.new(grid[0].length) { 0 }}
  (0...grid.length).each do |y|
    (0...grid[0].length).each do |x|
      next unless grid[y][x]
      new_grid[y][x+1] += 1 if x != grid[0].length - 1
      new_grid[y+1][x] += 1 if y != grid.length - 1
      new_grid[y+1][x+1] += 1 if y != grid.length - 1 && x != grid[0].length - 1
      new_grid[y-1][x] += 1 if y != 0
      new_grid[y][x-1] += 1 if x != 0
      new_grid[y-1][x-1] += 1 if x != 0 && y != 0
      new_grid[y+1][x-1] += 1 if x != 0 && y != grid.length - 1
      new_grid[y-1][x+1] += 1 if y != 0 && x != grid[0].length - 1
    end
  end

  new_grid.each_with_index.map do |row, y|
    row.each_with_index.map do |cell, x|
      if grid[y][x]
        cell == 2 || cell == 3
      else
        cell == 3
      end
    end
  end
end


100.times do |i|
  lights = update(lights)
end

puts lights.map { |row| row.count(true) }.inject(:+)
