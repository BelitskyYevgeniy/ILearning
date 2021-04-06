require 'sha3'
for fileName in Dir.children(Dir.getwd) do
    file=File.new(fileName, "r:UTF-8")
    puts fileName+"\t"+SHA3::Digest.new(256).hexdigest(file.read)
    file.close()
end