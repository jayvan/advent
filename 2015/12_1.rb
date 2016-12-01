#! /usr/bin/env ruby
require 'json'

def sum(json)
  case json
  when Array
    json.map { |el| sum(el) }.inject(:+)
  when Hash
    json.map { |key, val| sum(val) }.inject(:+)
  when Fixnum
    json
  else
    0
  end
end

json = JSON.parse(File.open(ARGV[0]).read)
puts sum(json)
