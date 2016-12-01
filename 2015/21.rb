#!/usr/bin/env ruby

def player_wins(boss_hp, boss_dmg, boss_armor, player_dmg, player_armor)
  player_turns = (boss_hp.to_f / [player_dmg - boss_armor, 1].max).ceil
  boss_turns = (100.0 / [boss_dmg - player_armor, 1].max).ceil

  player_turns <= boss_turns
end

boss_hp, boss_dmg, boss_def = ARGF.readlines.map { |line| line.split(' ').last.to_i }

weapons =
  [[8, 4, 0],
   [10, 5, 0],
   [25, 6, 0],
   [40, 7, 0],
   [74, 8, 0]]

armors =
  [[0, 0, 0],
   [13, 0, 1],
   [31, 0, 2],
   [53, 0, 3],
   [75, 0, 4],
   [102, 0, 5]]

rings =
  [[0, 0, 0],
   [0, 0, 0],
   [25, 1, 0],
   [50, 2, 0],
   [100, 3, 0],
   [20, 0, 1],
   [40, 0, 2],
   [80, 0, 3]]

cheapest = 2**16
most_expensive = 0

weapons.each do |weapon|
  armors.each do |armor|
    rings.combination(2).each do |ring_set|
      cost = weapon[0] + armor[0] + ring_set[0][0] + ring_set[1][0]
      player_dmg = weapon[1] + armor[1] + ring_set[0][1] + ring_set[1][1]
      player_def = weapon[2] + armor[2] + ring_set[0][2] + ring_set[1][2]

      player_turns = (boss_hp.to_f / [player_dmg - boss_def, 1].max).ceil
      boss_turns = (100.0 / [boss_dmg - player_def, 1].max).ceil
      if player_turns <= boss_turns
        cheapest = [cheapest, cost].min
      else
        most_expensive = [most_expensive, cost].max
      end
    end
  end
end

puts cheapest
puts most_expensive
