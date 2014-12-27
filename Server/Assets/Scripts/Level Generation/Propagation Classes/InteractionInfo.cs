#region License

// // InteractionInfo.cs
// //  
// //  Author:
// //        <colin.p.swensonh@gmail.com>
// // 
// //  Copyright (c) 2013 swensonhcp
// // 
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// // 
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU General Public License for more details.
// // 
// //  You should have received a copy of the GNU General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

public class InteractionInfo
{
    public enum status
    {
        Alopecia,
        Anti_Bodies_Metabolism,
        Choking,
        Coughing,
        Confusion,
        Deafness,
        Dizziness,
        Facial_Hypertrichosis,
        Fever,
        Headache,
        Itching,
        Longevity,
        Shivering,
        Sneezing,
        Stimulant,
        Toxic_Compensation,
        Vomiting,
        Vomiting_Blood,
        Weight_Gain,
        Weight_Loss,
        Weight_Even,
        Eternal_Youth,
        Hallucination,
        Hyphema,
        Necrotizing_Facitis,
        Self_Respiration,
        Toxin_Filter,
        Voice_Change
    };

    public float face;
    public float feet;
    public float hands;
    public float ingestion;
    public float injection;
    public float lungs;
    public float transmissionDistance;
}