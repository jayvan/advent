#! /usr/bin/env ruby
SECONDS = 2503

reindeer = []

ARGF.each_line do |line|
  data = line[0..-3].split(' ')
  reindeer << {
    speed: data[3].to_i,
    burst_length: data[6].to_i,
    rest_length: data[-2].to_i,
    mode: :bursting,
    duration: data[6].to_i,
    distance: 0,
    score: 0
  }
end

SECONDS.times do
  longest = 0
  reindeer.each do |donkey|
    donkey[:distance] += donkey[:speed] if donkey[:mode] == :bursting
    longest = [longest, donkey[:distance]].max
    donkey[:duration] -= 1

    next unless donkey[:duration] == 0
    if donkey[:mode] == :bursting
      donkey[:mode] = :resting
      donkey[:duration] = donkey[:rest_length]
    else
      donkey[:mode] = :bursting
      donkey[:duration] = donkey[:burst_length]
    end
  end

  reindeer.select { |d| d[:distance] == longest }.each do |donkey|
    donkey[:score] += 1
  end
end

puts reindeer.map { |d| d[:score] }.max
