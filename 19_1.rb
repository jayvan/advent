#! /usr/bin/env ruby
require 'set'

def substitutions(initial, source, target)
  (0...initial.length).map do |i|
    if initial[i..-1].start_with?(source)
      initial[0...i] + target + initial[(i + source.length)..-1]
    else
      nil
    end
  end.compact
end

replacements = Hash.new { |hash, v| hash[v] = [] }
final_line = false
initial = ''
ARGF.each_line do |line|
  line.chomp!
  if line.empty?
    final_line = true
    next
  end

  if final_line
    initial = line
  else
    data = line.chomp.split(' ')
    replacements[data[0]] << data[2]
  end
end

outcomes = Set.new

replacements.each do |source, targets|
  targets.each do |target|
    outcomes.merge(substitutions(initial, source, target))
  end
end

puts outcomes.length
