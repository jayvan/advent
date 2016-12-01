#! /usr/bin/env ruby

lights = Array.new(1000) { Array.new(1000) { 0 } }
ARGF.each_line do |line|
  components = line.split(' ')
  command = if line =~ /^turn on/
    :on
  elsif line =~ /^turn off/
    :off
  else
    :toggle
  end

  if command == :toggle
    x1, y1 = components[1].split(',').map(&:to_i)
    x2, y2 = components[3].split(',').map(&:to_i)
  else
    x1, y1 = components[2].split(',').map(&:to_i)
    x2, y2 = components[4].split(',').map(&:to_i)
  end

  x1.upto(x2).each do |x|
    y1.upto(y2).each do |y|
      case command
      when :on
        lights[x][y] += 1
      when :off
        lights[x][y] = [0, lights[x][y] - 1].max
      else
        lights[x][y] += 2
      end
    end
  end
end

puts "Answer: #{lights.map { |row| row.inject(:+) }.inject(:+)}"
