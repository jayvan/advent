#! /usr/bin/env ruby

lights = Array.new(1000) { Array.new(1000) { false } }
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
        lights[x][y] = true
      when :off
        lights[x][y] = false
      else
        lights[x][y] = !lights[x][y]
      end
    end
  end
end

puts "Answer: #{lights.map { |row| row.count(true) }.inject(:+)}"
