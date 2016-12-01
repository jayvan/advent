#! /usr/bin/env ruby
require 'json'

def sum(json)
  case json
  when Array
    json.map { |el| sum(el) }.inject(:+)
  when Hash
    if json.values.include?('red')
      0
    else
      json.map { |key, val| sum(val) }.inject(:+)
    end
  when Fixnum
    json
  else
    0
  end
end

json = JSON.parse(File.open(ARGV[0]).read)
puts sum(json)
