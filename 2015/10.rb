#! /usr/bin/env ruby

def step(nums)
  result = []
  current = nums[0]
  count = 1

  nums[1..-1].each do |num|
    if num == current
      count += 1
    else
      result.push(count, current)
      current = num
      count = 1
    end
  end

  result.push(count, current)
  result
end

ARGF.each_line do |key|
  nums = key.chomp.chars.map(&:to_i)
  50.times do |i|
    nums = step(nums)
    puts "#{i + 1}: #{nums.length}"
  end
end
