#! /usr/bin/env ruby
require 'digest'

ARGF.each_line do |key|
  key.chomp!
  counter = 1
  loop do
    if Digest::MD5.hexdigest("#{key}#{counter}")[0...6] == '000000'
      puts "Answer: #{counter}"
      exit
    end
    counter += 1
  end
end
