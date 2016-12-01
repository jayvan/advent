#! /usr/bin/env ruby

answer = 0

ARGF.each_line do |line|
  next unless line =~ /([aeiou].*){3}/
  next unless line =~ /(.)\1/
  next if line =~ /(ab|cd|pq|xy)/

  answer += 1
end

puts "Answer: #{answer}"
