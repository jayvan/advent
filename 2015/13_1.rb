#! /usr/bin/env ruby

moods = Hash.new { |hash, key| hash[key] = {} }
ARGF.each_line do |line|
  data = line[0..-3].split(' ')
  score = data[3].to_i
  score *= -1 if data[2] != 'gain'
  moods[data[0]][data[-1]] = score
end

best = -2**32
moods.keys.to_a.permutation.each do |perm|
  happiness = 0
  perm.each_with_index do |person, index|
    happiness += moods[person][perm[index-1]]
    happiness += moods[perm[index-1]][person]
  end

  best = [happiness, best].max
end

puts best
