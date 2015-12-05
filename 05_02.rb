#! /usr/bin/env ruby

answer = 0

ARGF.each_line do |line|
  next unless line =~ /(..).*\1/
  next unless line =~ /(.).\1/

  answer += 1
end

puts "Answer: #{answer}"
