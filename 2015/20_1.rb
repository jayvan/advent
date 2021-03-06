#! /usr/bin/env ruby

def num_presents(house_num)
  presents = 0
  1.upto(Math.sqrt(house_num)) do |elf_num|
    if house_num % elf_num == 0
      presents += elf_num * 10
      other_elf = house_num / elf_num
      presents += 10 * other_elf unless other_elf == elf_num
    end
  end

  presents
end

house_number = 1
loop do
  if num_presents(house_number) >= 36_000_000
    puts 'DONE'
    puts house_number
    break
  end

  if house_number % 1000 == 0
    puts "#{house_number}: #{num_presents(house_number)}"
  end
  house_number += 1
end


