#! /usr/bin/env ruby
SECONDS = 2503
best = 0

ARGF.each_line do |line|
  data = line[0..-3].split(' ')
  speed = data[3].to_i
  burst = data[6].to_i
  rest = data[-2].to_i

  cycle_length = burst + rest
  whole_cycles = SECONDS / cycle_length
  final_sprint = [burst, SECONDS - whole_cycles * cycle_length].min
  distance = whole_cycles * burst * speed + final_sprint * speed
  best = [best, distance].max
end

puts best
