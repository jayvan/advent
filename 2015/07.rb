#! /usr/bin/env ruby

$gates = Hash.new { |hash, key| hash[key] = Proc.new { key.to_i }}

$computed = {}
def get_computed(gate)
  return $computed[gate] if $computed[gate]
  $computed[gate] = $gates[gate].call
  $computed[gate]
end

ARGF.each_line do |line|
  data = line.split(' ')
  if data.length == 3
    $gates[data[2]] = Proc.new {|n| get_computed(data[0])}
  elsif data.length == 5
    if data[1] == 'AND'
      $gates[data[4]] = Proc.new {|n|  get_computed(data[0]) & get_computed(data[2]) }
    elsif data[1] == 'OR'
      $gates[data[4]] = Proc.new {|n|  get_computed(data[0]) | get_computed(data[2]) }
    elsif data[1] == 'LSHIFT'
      $gates[data[4]] = Proc.new {|n|  (get_computed(data[0]) << data[2].to_i) }
    elsif data[1] == 'RSHIFT'
      $gates[data[4]] = Proc.new {|n|  (get_computed(data[0]) >> data[2].to_i) }
    end
  elsif data.length == 4
    $gates[data[3]] = Proc.new {|n|  (~get_computed(data[1])) & 65535 }
  end
end

new_a = get_computed('a')
$computed = {}
$computed['b'] = new_a

puts "Part 1: #{new_a}"
puts "Part 2: #{get_computed('a')}"
