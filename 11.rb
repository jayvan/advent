#! /usr/bin/env ruby

def valid?(password)
  return false if password =~ /(i|o|l)/

  return false unless password =~ /(.)\1.*(.)\2/

  has_sequence = false
  2.upto(password.length - 1) do |i|
    if password[i] == password[i-1].next && password[i-1] == password[i-2].next
      has_sequence = true
      break
    end
  end

  has_sequence
end

def step(password)
  password = password.next
  invalid_char = password =~ /(i|o|l)/
  if invalid_char
    password[invalid_char] = password[invalid_char].next
    password[invalid_char+1..-1] = 'a' * (password.length - invalid_char - 1)
  end
  password
end

ARGF.each_line do |password|
  password.chomp!
  loop do
    password = step(password)
    break if valid?(password)
  end
  puts "Answer: #{password}"
end

