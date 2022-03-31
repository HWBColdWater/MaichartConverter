namespace MaichartConverter
{
    public abstract class Chart : IChart, ICompiler
    {
        //Stores all notes
        private List<Note> notes;

        //Stores definitions of BPM Changes
        private BPMChanges bpmChanges;

        //Stores definitions of Measure Changes
        private MeasureChanges measureChanges;

        //Counts number of Tap
        private int tapNumber;

        //Counts number of Break
        private int breakNumber;

        //Counts number of Hold
        private int holdNumber;

        //Counts number of Slide
        private int slideNumber;

        //Counts number of Touch
        private int touchNumber;

        //Counts number of Touch Hold
        private int thoNumber;

        //Defines 
        private readonly int[] unitScore = { 500, 1000, 1500, 2500 };
        private int achievement = 0;
        private int totalDelay = 0;
        private List<List<Note>> chart;
        private TrackInformation information;
        private readonly string[] TapTypes = { "TAP", "STR", "TTP", "XTP", "XST" };
        private readonly string[] HoldTypes = { "HLD", "THO", "XHO" };
        private readonly string[] SlideTypes = { "SI_", "SV_", "SF_", "SCL", "SCR", "SUL", "SUR", "SLL", "SLR", "SXL", "SXR", "SSL", "SSR" };

        ///Theoretical Rating = (Difference in 100-down and Max score)/100-down
        /// <summary>
        /// Access to Notes
        /// </summary>
        public List<Note> Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes=value;
            }
        }

        /// <summary>
        /// Returns this.Chart. aka List of bars
        /// </summary>
        /// <value>this.Chart</value>
        public List<List<Note>> StoredChart
        {
            get 
            { 
                return this.chart; 
            }
            set 
            {
                this.chart=value;
            }
        }

        /// <summary>
        /// Access to BPM Changes
        /// </summary>
        public BPMChanges BPMChanges
        {
            get
            {
                return this.bpmChanges;
            }
            set
            {
                this.bpmChanges = value;
            }
        }

        /// <summary>
        /// Access to Measure Changes
        /// </summary>
        public MeasureChanges MeasureChanges
        {
            get
            {
                return this.measureChanges;
            }
            set
            {
                this.measureChanges = value;
            }
        }

        /// <summary>
        /// Access to Tap Number
        /// </summary>
        public int TapNumber
        {
            get
            {
                return this.tapNumber;
            }
            set
            {
                this.tapNumber = value;
            }
        }

        /// <summary>
        /// Access to Break Number
        /// </summary>
        public int BreakNumber
        {
            get
            {
                return this.breakNumber;
            }
            set
            {
                this.breakNumber = value;
            }
        }

        /// <summary>
        /// Access to Hold Number
        /// </summary>
        public int HoldNumber
        {
            get
            {
                return this.holdNumber;
            }
            set
            {
                this.holdNumber = value;
            }
        }

        /// <summary>
        /// Access to Slide Number
        /// </summary>
        public int SlideNumber
        {
            get
            {
                return this.slideNumber;
            }
            set
            {
                this.slideNumber = value;
            }
        }

        /// <summary>
        /// Access to Touch Number
        /// </summary>
        public int TouchNumber
        {
            get
            {
                return this.touchNumber;
            }
            set
            {
                this.touchNumber = value;
            }
        }

        /// <summary>
        /// Access to Touch Hold Number
        /// </summary>
        public int ThoNumber
        {
            get
            {
                return this.thoNumber;
            }
            set
            {
                this.thoNumber = value;
            }
        }

        /// <summary>
        /// Access to Unit Score
        /// </summary>
        public int[] UnitScore
        {
            get
            {
                return this.UnitScore;
            }
        }

        /// <summary>
        /// Access to theoretical Achievement
        /// </summary>
        public int Achievement
        {
            get
            {
                return this.Achievement;
            }
            set
            {
                this.Achievement = value;
            }
        }

        /// <summary>
        /// Return the total delayed value of this Chart.
        /// </summary>
        /// <value>this.TotalDelayedValue</value>
        public int TotalDelay
        {
            get
            {
                return this.totalDelay;
            }
            set
            {
                this.totalDelay = value;
            }
        }

        /// <summary>
        /// Return Information
        /// </summary>
        /// <value>this.Information</value>
        public TrackInformation Information
        {
            get
            {
                return this.information;
            }
            set
            {
                this.information = value;
            }
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Chart()
        {
            this.notes = new List<Note>();
            this.bpmChanges = new BPMChanges();
            this.measureChanges = new MeasureChanges();
            this.chart = new List<List<Note>>();
            this.information = new TrackInformation();
        }

        /// <summary>
        /// Check if every item is valid for exporting
        /// </summary>
        /// <returns>True if every element is valid, false elsewise</returns>
        public bool CheckValidity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update properties in Good Brother for exporting
        /// </summary>
        public virtual void Update()
        {
            int maxBar = notes[notes.Count - 1].Bar;
            for (int i = 0; i <= maxBar; i++)
            {
                List<Note> bar = new List<Note>();
                BPMChange noteChange = new BPMChange();
                double currentBPM = this.BPMChanges.ChangeNotes[0].BPM;
                Note lastNote = new Rest();
                foreach (BPMChange x in this.BPMChanges.ChangeNotes)
                {
                    if (x.Bar == i)
                    {
                        bar.Add(x);
                    }
                }
                foreach (Note x in this.Notes)
                {
                    if (x.Bar == i)
                    {
                        int delay = x.Bar * 384 + x.Tick + x.WaitTime + x.LastTime;
                        switch (x.NoteSpecificType)
                        {
                            case "BPM":
                                currentBPM = x.BPM;
                                break;
                            case "MEASURE":
                                break;
                            case "REST":
                                break;
                            case "TAP":
                                this.tapNumber++;
                                if (x.NoteType.Equals("TTP"))
                                {
                                    this.touchNumber++;
                                }
                                else if (x.NoteType.Equals("BRK") || x.NoteType.Equals("BST"))
                                {
                                    this.breakNumber++;
                                }
                                break;
                            case "HOLD":
                                this.holdNumber++;
                                this.slideNumber++;
                                if (delay > this.TotalDelay)
                                {
                                    this.totalDelay = delay;
                                    //Console.WriteLine("New delay: " + delay);
                                    //Console.WriteLine(x.Compose(1));
                                }
                                if (x.NoteType.Equals("THO"))
                                {
                                    this.thoNumber++;
                                }
                                break;
                            case "SLIDE_START":
                                this.tapNumber++;
                                break;
                            case "SLIDE":
                                this.slideNumber++;
                                if (delay > this.TotalDelay)
                                {
                                    this.totalDelay = delay;
                                    //Console.WriteLine("New delay: "+delay);
                                    //Console.WriteLine(x.Compose(1));
                                }
                                break;
                            default:
                                break;
                        }
                        x.BPM = currentBPM;
                        x.Prev = lastNote;
                        lastNote.Next = x;
                        bar.Add(x);
                        if (!x.NoteSpecificType.Equals("SLIDE"))
                        {
                            lastNote = x;
                        }
                    }
                }

                List<Note> afterBar = new List<Note>();
                afterBar.Add(new MeasureChange(i, 0, CalculateQuaver(CalculateLeastMeasure(bar))));
                //Console.WriteLine();
                //Console.WriteLine("In bar "+i+", LeastMeasure is "+ CalculateLeastMeasure(bar)+", so quaver will be "+ CalculateQuaver(CalculateLeastMeasure(bar)));
                afterBar.AddRange(bar);
                this.chart.Add(FinishBar(afterBar, this.BPMChanges.ChangeNotes, i, CalculateQuaver(CalculateLeastMeasure(bar))));
            }
            //Console.WriteLine("TOTAL DELAY: "+this.TotalDelay);
            //Console.WriteLine("TOTAL COUNT: "+ this.chart.Count * 384);
            if (this.totalDelay<this.chart.Count*384)
            {
                this.totalDelay = 0;
            }
            else
            {
                this.totalDelay -= this.chart.Count * 384;
            }
        }

        /// <summary>
        /// Compose chart in appropriate result.
        /// </summary>
        /// <returns>String of chart compiled</returns>
        public abstract string Compose();

        /// <summary>
        /// Override and compose with given arrays
        /// </summary>
        /// <param name="bpm">Override BPM array</param>
        /// <param name="measure">Override Measure array</param>
        /// <returns>Good Brother with override array</returns>
        public abstract string Compose(BPMChanges bpm, MeasureChanges measure);

        /// <summary>
        /// Return the least none 0 measure of bar.
        /// </summary>
        /// <param name="bar">bar to take in</param>
        /// <returns>List none 0 measure</returns>
        public static int CalculateLeastMeasure(List<Note> bar)
        {
            List<int> startTimeList = new List<int>();
            startTimeList.Add(0);
            foreach (Note x in bar)
            {
                if (!startTimeList.Contains(x.Tick))
                {
                    startTimeList.Add(x.Tick);
                }
                if (x.NoteType.Equals("BPM"))
                {
                    //Console.WriteLine(x.Compose(0));
                }
            }
            if (startTimeList[startTimeList.Count - 1] != 384)
            {
                startTimeList.Add(384);
            }
            List<int> intervalCandidates = new List<int>();
            int minimalInterval = GCD(startTimeList[0],startTimeList[1]);
            for (int i = 1; i < startTimeList.Count; i++)
            {
                minimalInterval = GCD(minimalInterval, startTimeList[i]);
            }
            return minimalInterval;
        }

        /// <summary>
        /// Return note number except Rest, BPM and Measure.
        /// </summary>
        /// <param name="Bar">bar of note to take in</param>
        /// <returns>Number</returns>
        public static int RealNoteNumber(List<Note> Bar)
        {
            int result = 0;
            foreach (Note x in Bar)
            {
                if (x.IsNote)
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// Judges if this bar contains notes
        /// </summary>
        /// <param name="Bar">Bar to analyze on</param>
        /// <returns>True if contains, false elsewise</returns>
        public static bool ContainNotes(List<Note> Bar)
        {
            bool result = false;
            foreach (Note x in Bar)
            {
                result = result || x.IsNote;
            }
            return result;
        }

        /// <summary>
        /// Generate appropriate length for hold and slide.
        /// </summary>
        /// <param name="length">Last Time</param>
        /// <returns>[Definition:Length]=[Quaver:Beat]</returns>
        public static int CalculateQuaver(int length)
        {
            int result = 0;
            const int definition = 384;
            int divisor = GCD(definition, length);
            int quaver = definition / divisor, beat = length / divisor;
            result = quaver;
            return result;
        }

        /// <summary>
        /// Finish Bar writing byu adding specific rest note in between.
        /// </summary>
        /// <param name="bar">Bar to finish with</param>
        /// <param name="bpmChanges">BPMChange Notes</param>
        /// <param name="barNumber">Bar number of Bar</param>
        /// <param name="minimalQuaver">Minimal interval calculated from bar</param>
        /// <returns>Finished bar</returns>
        public static List<Note> FinishBar(List<Note> bar, List<BPMChange> bpmChanges, int barNumber, int minimalQuaver)
        {
            List<Note> result = new List<Note>();
            bool writeRest = true;
            result.Add(bar[0]);
            for (int i = 0; i < 384; i += 384 / minimalQuaver)
            {
                //Separate Touch and others to prevent ordering issue
                Note bpm = new Rest();
                List<Note> eachSet = new List<Note>();
                List<Note> touchEachSet = new List<Note>();
                //Set condition to write rest if appropriate
                writeRest = true;
                //Add Appropriate note into each set
                foreach (Note x in bar)
                {
                    if ((x.Tick == i) && x.IsNote&& !(x.NoteType.Equals("TTP")|| x.NoteType.Equals("THO")))
                    {
                        if (x.NoteSpecificType.Equals("BPM"))
                        {
                            bpm = x;
                            //List<Note> tempSet = new List<Note>();
                            //tempSet.Add(x);
                            //tempSet.AddRange(eachSet);
                            //eachSet=tempSet;
                            //Console.WriteLine("A note was found at tick " + i + " of bar " + barNumber + ", it is "+x.NoteType);
                        }
                        else
                        {
                            eachSet.Add(x);
                            //Console.WriteLine("A note was found at tick " + i + " of bar " + barNumber + ", it is "+x.NoteType);
                            writeRest = false;
                        }                      
                    }
                    else if ((x.Tick == i) && x.IsNote&& (x.NoteType.Equals("TTP") || x.NoteType.Equals("THO")))
                    {
                        if (x.NoteSpecificType.Equals("BPM"))
                        {
                            bpm = x;
                            //List<Note> tempSet = new List<Note>();
                            //tempSet.Add(x);
                            //tempSet.AddRange(touchEachSet);
                            //touchEachSet=tempSet;
                            //Console.WriteLine("A note was found at tick " + i + " of bar " + barNumber + ", it is "+x.NoteType);
                        }
                        else
                        {
                            touchEachSet.Add(x);
                            //Console.WriteLine("A note was found at tick " + i + " of bar " + barNumber + ", it is "+x.NoteType);
                            writeRest = false;
                        }
                    }
                }
                //Searching for BPM change. If find one, get into front.
                if (bpm.BPM!=0)
                {
                    List<Note> adjusted = new List<Note>();
                    adjusted.Add(bpm);
                    adjusted.AddRange(touchEachSet);
                    adjusted.AddRange(eachSet);
                    eachSet = adjusted;
                }
                else
                {
                    List<Note> adjusted = new List<Note>();
                    adjusted.AddRange(touchEachSet);
                    adjusted.AddRange(eachSet);
                    eachSet = adjusted;
                }
                //foreach (BPMChange x in bpmChanges)
                //{
                //    if (x.Bar == barNumber && x.Tick == i)
                //    {
                //        List<Note> adjusted = new List<Note>();
                //        eachSet.Remove(x);
                //        touchEachSet.Remove(x);
                //        adjusted.Add(x);
                //        adjusted.AddRange(touchEachSet);
                //        adjusted.AddRange(eachSet);
                //        eachSet = adjusted;
                //        addedTouch = true;
                //    }
                //    //if (eachSet.Contains(x) && !addedTouch)
                //    //{
                //    //    eachSet.Remove(x);
                //    //    List<Note> adjusted = new List<Note>();
                //    //    adjusted.Add(x);
                //    //    adjusted.AddRange(touchEachSet);
                //    //    adjusted.AddRange(eachSet);
                //    //    eachSet = adjusted;
                //    //    addedTouch = true;
                //    //}
                //    //else if (touchEachSet.Contains(x)&&!addedTouch)
                //    //{
                //    //    touchEachSet.Remove(x);
                //    //    List<Note> adjusted = new List<Note>();
                //    //    adjusted.Add(x);
                //    //    adjusted.AddRange(touchEachSet);
                //    //    adjusted.AddRange(eachSet);
                //    //    eachSet = adjusted;
                //    //    addedTouch = true;
                //    //}
                //    //else if (!addedTouch)
                //    //{
                //    //    List<Note> adjusted = new List<Note>();
                //    //    adjusted.AddRange(touchEachSet);
                //    //    adjusted.AddRange(eachSet);
                //    //    eachSet = adjusted;
                //    //    addedTouch = true;
                //    //}
                //}
                if (writeRest)
                {
                    //Console.WriteLine("There is no note at tick " + i + " of bar " + barNumber + ", Adding one");
                    eachSet.Add(new Rest("RST", barNumber, i));
                }
                result.AddRange(eachSet);
            }
            if (RealNoteNumber(result) != RealNoteNumber(bar))
            {
                string error = "";
                error += ("Bar notes not match in bar: " + barNumber)+"\n";
                error += ("Expected: " + RealNoteNumber(bar)) + "\n";
                foreach (Note x in bar)
                {
                    error += (x.Compose(1)) + "\n";
                }
                error += ("\nActual: " + RealNoteNumber(result)) + "\n";
                foreach (Note y in result)
                {
                    error += (y.Compose(1)) + "\n";
                }
                Console.WriteLine(error);
                throw new Exception("NOTE NUMBER IS NOT MATCHING");
            }
            //result.Sort();
            //if (RealNoteNumber(result)==0)
            //{
            //    Console.WriteLine("There is no note at tick " + 0 + " of bar " + barNumber + ", Adding one");
            //    result.Add(new Rest("RST", barNumber,0));
            //}
            // if (result[1].NoteSpecificType().Equals("BPM"))
            // {
            //     Note temp = result[0];
            //     result[0] = result[1];
            //     result[1] = temp;
            // }
            bool hasFirstBPMChange=false;
            List<Note> changedResult = new List<Note>();
            Note potentialFirstChange = new Rest();
            {
                for(int i = 0;!hasFirstBPMChange&&i<result.Count();i++)
                {
                    if (result[i].NoteGenre.Equals("BPM")&&result[i].Tick==0)
                    {                    
                        changedResult.Add(result[i]);
                        potentialFirstChange = result[i];
                        hasFirstBPMChange=true;
                    }
                }
                if (hasFirstBPMChange)
                {
                    result.Remove(potentialFirstChange);
                    changedResult.AddRange(result);
                    result=changedResult;
                }
            }

            return result;
        }

        /// <summary>
        /// Return GCD of A and B.
        /// </summary>
        /// <param name="a">A</param>
        /// <param name="b">B</param>
        /// <returns>GCD of A and B</returns>
        public static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        /// <summary>
        /// Return if this is a prime (1 counts)
        /// </summary>
        /// <param name="number">Number to inspect</param>
        /// <returns>True if is prime, false elsewise</returns>
        public static bool IsPrime(int number)
        {
            if (number < 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        /// <summary>
        /// Take In Information
        /// </summary>
        /// <param name="newInformation">New information to take in.</param>
        public void TakeInformation(TrackInformation newInformation)
        {
            this.Information = newInformation;
        }
    }
}