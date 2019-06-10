using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PatternGenerator {

  public static Pattern RightLine(int pl, int max, bool fill) {
      var thisPattern = new Pattern();
      thisPattern.Lines = new List<Line>();

      for (int lineIndex = 0; lineIndex < max; lineIndex++) {
        var thisLine = new Line();
        thisLine.Spots = new List<bool>();

        for (int spotIndex = 0; spotIndex < max; spotIndex++) {
          // this is the crux of the pattern
          var rightOfPlayer =  PatternGenerator.CorrectIndex(pl - lineIndex, max);

          if (rightOfPlayer == spotIndex) {
            thisLine.Spots.Add(!fill);
          } else {
            thisLine.Spots.Add(fill);
          }
        }

        thisLine.WaitTime= (fill) ? 1f : .5f;
        thisPattern.Lines.Add(thisLine);     
      }
      return thisPattern;
    }

    public static Pattern LeftLine(int pl, int max, bool fill) {

      var thisPattern = new Pattern();
      thisPattern.Lines = new List<Line>();

      for (int lineIndex = 0; lineIndex < max; lineIndex++) {
        var thisLine = new Line();
        thisLine.Spots = new List<bool>();

        for (int spotIndex = 0; spotIndex < max; spotIndex++) {
          var leftOfPlayer =  PatternGenerator.CorrectIndex(pl + lineIndex, max);

          if (leftOfPlayer == spotIndex) {
            thisLine.Spots.Add(!fill);
          } else {
            thisLine.Spots.Add(fill);
          }
        }

        thisLine.WaitTime= (fill) ? 1f : .5f;
        thisPattern.Lines.Add(thisLine);
      }
      return thisPattern;
    }

    public static Pattern FlyingV(int pl, int max, bool fill) {
      var thisPattern = new Pattern();
      thisPattern.Lines = new List<Line>();

      for (int lineIndex = 0; lineIndex < max + 1; lineIndex++) {
        var thisLine = new Line();
        thisLine.Spots = new List<bool>();

        for (int spotIndex = 0; spotIndex < max; spotIndex++) {
          var rightOfPlayer = PatternGenerator.CorrectIndex(pl + lineIndex, max);
          var leftOfPlayer = PatternGenerator.CorrectIndex(pl - lineIndex, max);

          if (rightOfPlayer == spotIndex || leftOfPlayer == spotIndex) {
            thisLine.Spots.Add(!fill);
          } else {
            thisLine.Spots.Add(fill);
          }
        }

        thisLine.WaitTime= (fill) ? 1f : .5f;
        thisPattern.Lines.Add(thisLine);
      }
      return thisPattern;
    }

    private static int CorrectIndex(int value, int max) {
      if (value >= max) {
        return Mathf.Abs(max - value);
      }
      if (value < 0) {
        return max + value;
      }
      else {
        return value;
      }
    }
}
