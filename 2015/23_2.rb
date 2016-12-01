#!/usr/bin/env ruby

registers = {
  'a' => 1,
  'b' => 0
}

pc = 0

instructions = ARGF.readlines.map { |line| line.gsub(',', '').split(' ') }

while pc >= 0 && pc < instructions.length
  instruction = instructions[pc]
  case instruction[0]
  when 'hlf'
    registers[instruction[1]] /= 2
    pc += 1
  when 'tpl'
    registers[instruction[1]] *= 3
    pc += 1
  when 'inc'
    registers[instruction[1]] += 1
    pc += 1
  when 'jmp'
    pc += instruction[1].to_i
  when 'jie'
    if registers[instruction[1]].even?
      pc += instruction[2].to_i
    else
      pc += 1
    end
  when 'jio'
    if registers[instruction[1]] == 1
      pc += instruction[2].to_i
    else
      pc += 1
    end
  end
  puts "#{pc + 1}: #{registers.inspect}"
end
