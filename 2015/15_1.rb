#! /usr/bin/env ruby
AMOUNT = 100

ingredients = []
ARGF.each_line do |line|
 data = line.chomp.split(' ').map(&:to_i)
 ingredients << [data[2], data[4], data[6], data[8]]
end

def score(ingredients, amounts)
  scores = ingredients.each_with_index.map do |ingredient, i|
    ingredient.map { |quality| quality * amounts[i] }
  end

  sums = scores.transpose.map {|x| x.inject(:+)}
  return 0 if sums.any? { |x| x <= 0 }
  sums.inject(:*)
end

def search(ingredients, ingredient = 0, remaining = AMOUNT, amounts = [])
  if ingredient == ingredients.length - 1
    return score(ingredients, amounts + [remaining])
  end

  0.upto(remaining).map do |i|
    search(ingredients, ingredient + 1, remaining - i, amounts + [i])
  end.max
end

puts search(ingredients)
