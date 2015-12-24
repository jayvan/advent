#!/usr/bin/env ruby

packages = []

ARGF.each_line do |line|
  packages << line.to_i
end

one_fourth = packages.inject(:+) / 4
done = false
lowest_entanglement = 2**64

def can_divide?(num_groups, sum, packages)
  return packages.inject(:+) == sum if num_groups == 1

  1.upto(packages.length) do |size|
    packages.combination(size) do |combo|
      next unless combo.inject(:+) == sum
      packages_left = packages - combo
      return true if can_divide?(num_groups - 1, sum, packages_left)
    end
  end

  false
end

1.upto(packages.length) do |size|
  packages.combination(size) do |combo|
    next unless combo.inject(:+) == one_fourth

    entanglement = combo.inject(:*)
    next unless entanglement < lowest_entanglement

    packages_left = packages - combo
    if can_divide?(3, one_fourth, packages_left)
      done = true
      lowest_entanglement = entanglement
    end
  end
  break if done
end

puts lowest_entanglement
