#! /usr/bin/env ruby

clues = {
  'children:' => 3,
  'cats:' => 7,
  'samoyeds:' => 2,
  'pomeranians:' => 3,
  'akitas:' => 0,
  'vizslas:' => 0,
  'goldfish:' => 5,
  'trees:' => 3,
  'cars:' => 2,
  'perfumes:' => 1
}

ARGF.each_line.each_with_index do |line, i|
  profile = Hash[*line.chomp.split(' ')[2..-1]]

  valid = profile.all? do |attribute, value|
    clues[attribute] == value.to_i
  end

  if valid
    puts i + 1
    break
  end
end
