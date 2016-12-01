#! /usr/bin/env ruby

in_code = 0
in_memory = 0
ARGF.each_line do |line|
  line.chomp!
  in_code += line.length
  in_memory += line.inspect.length
end

puts "Answer: #{in_memory - in_code}"
