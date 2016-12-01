#! /usr/bin/env ruby

in_code = 0
in_memory = 0
ARGF.each_line do |line|
  line.chomp!
  line = line[1..-2]
  in_memory += (eval %Q{"#{line}"}).length
  in_code += line.codepoints.length + 2
end

puts "Answer: #{in_code - in_memory}"
