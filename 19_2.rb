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

def possibilities(input, replacements)
  outcomes = Set.new

  replacements.each do |source, target|
    outcomes.merge(substitutions(input, source, target))
  end

  outcomes
end

replacements = {}
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
    replacements[data[2]] = data[0]
  end
end

transforms = 0
current = [initial]

while !current.include?('e')
  current = current.map do |a|
    possibilities(a, replacements)
  end.inject(:merge)
  current = current.sort { |a, b| a.length <=> b.length }.first(1)

  transforms += 1
end

puts transforms
