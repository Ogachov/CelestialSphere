class Converter:
    def __init__(self):
        self.numStar = 0
        self.threshold = 7.0

    def execute(self):
        self.numStar = 0
        print('do something')
        f = open('hip_main.dat')
        lines = f.readlines()
        f.close()

        f = open('starlist.dat', 'w')
        for line in lines:
#            print(line)
            sp = line.split('|')
            hipId = int(sp[1])

            try:
                RAdeg = float(sp[8])
            except ValueError:
                continue

            try:
                DEdeg = float(sp[9])
            except ValueError:
                continue

            magnitude = float(sp[5])
            spectrum = sp[76].strip()
            if len(spectrum) == 0:
                spectrum = 'W'
            else:
                spectrum = spectrum[0]

            if magnitude < self.threshold:
                text = f"{{{hipId}, new Info({RAdeg}, {DEdeg}, {magnitude}, '{spectrum}')}},\n"
                print(text)
                f.write(text)

                self.numStar += 1

        print(f"magnitude threshold = {self.threshold}  valid stars {self.numStar} / total stars {len(lines)}")
        f.close()

if __name__ == '__main__':
    converter = Converter()
    print('init converter')
    converter.execute()
