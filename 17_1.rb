#! /usr/bin/env ruby

containers = []
ARGF.each_line do |line|
  containers << line.to_i
end

combos = 0
1.upto(containers.length).each do |num|
  containers.combination(num).each do |combo|
    if combo.inject(:+) == 150
      combos += 1
    end
  end
end

puts combos
