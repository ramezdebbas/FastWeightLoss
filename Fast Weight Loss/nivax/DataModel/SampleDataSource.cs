using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "The Ways and Tips",
                    "The Ways and Tips",
                    "Assets/00.jpg",
                    "You know the drill when it comes to losing weight -- take in fewer calories, burn more calories. But you also know that most diets and quick weight-loss plans have about as much substance as a politician's campaign pledges. Here are ma easy many ways for you to finally lose the weight.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Answering the Big Question of Weight Loss: Supplements or No Supplements?",
                    "In today's high-tech day and age where medical science has been able to accomplish so much, there are numerous drugs surfacing here and there that claim to be the 'answer' to the problem of weight loss.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nMost of these term themselves as 'supplements' and each claim to hold the secret to helping people lose weight quickly and in the easiest way possible.\n\nBut do they work?\n\nTruth be told, many people have had numerous bad experiences with supplements and that has led to a number of widely circulated horror stories cropping up regarding them. As such, supplements are nowadays widely regarded as being nothing more than scams and are treated with a great deal of skepticism.\n\nStill, for every horror story there is an equal number of astounding success stories. Some people have reported experienced compelling weight loss when they take certain supplements, and this makes people wonder as to whether or not there is more to these supplements than the skeptics think.\n\nTo understand what supplements do, it is important that you understand how most work. In general, most supplements end up doing one or more of the following:\n\n1.  Aid in appetite control\n2.  Help boost metabolism rates\n3.  Assist the digestive system\n\nAs you should see, each of these three has the potential to aid weight loss.\n\nFor starters, appetite control is an important aspect of avoiding overeating or binge eating, as it is otherwise known. Many people who are overweight generally have problems controlling the amount that they eat, and so this is something that is truly valuable and can cut out that factor entirely.\n\nSimilarly, people with low metabolism rates are constantly at risk of gaining weight, and by giving them a quick boost to their metabolism rate, they're going to be better able to work through any excess energy that their body has and thus not store it as fat.\n\nLastly, assisting the digestive system has been known to really streamline its tasks and allow it to metabolize food a lot faster. In turn, this means that you're less likely to be metabolizing food while you sleep, and then just storing all the excess energy too.\n\nIn a nutshell, supplements that do any of the three above tasks which we've just discussed definitely have the potential to help you lose weight. However the big question is: Do they work as advertised.\n\nWhen it comes to that, there is no easy answer. Sure, most supplements can help, but if they have claims that are more outlandish than realistic, then they probably aren't going to work as you expect them to.\n\nWhen used to aid weight loss, supplements can be a valuable tool, but at the end of the day if they're not accompanied by other efforts to lose weight, you're probably not going to get very far.\n\nBottom line: Supplements can be a help, but they're no 'miracle cure'.",
                    group1) { CreatedOn = "Group", CreatedTxt = "The Ways and Tips", CreatedOnTwo = "Item", CreatedTxtTwo = "Supplements or No Supplements", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/01.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Total Body Exercises that Will Dramatically Help Weight Loss",
                     "Know what the problem with losing weight is? More often that not even after you actually begin to succeed, you find that you're actually not really succeeding quite the way that you hoped you would be.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nWhile certain parts of your body may certainly be slimming down, others will stay stubbornly flabby, and this can make your shape look even a little worse than you thought it was to begin with.\n\nMainly, the reason why this happens in the first place is because your body isn't really programmed to lose weight uniformly. Instead, it loses weight in direct response to what you're doing and which parts of your body you're working most.\n\nDue to that, if you're doing something like walking or running, you'll find that it's your legs, and possibly arms that end up slimming down the most, whereas your belly area and even inner thighs will not slim down as much.\n\nNaturally, the solution to this is simple: Total body exercises.\n\nSome exercises out there are just better at encouraging uniform weight loss than others, mostly because they use a wide array of muscles throughout the body and thus help you to mold yourself into a more desirable shape.\n\nUsing a wide array of muscles also has the dual advantage of toning the muscles in those areas too, which means that whatever flab does remain is better supported by the lean muscle bulk that you're going to be building up there too. End of the day, this is going to help you even more than the actual fat loss will.\n\nHow about we take a look at three types of exercises in particular that will help you with a total body workout:\n\n1.  Swimming\nHow many times have you enviously looked at the perfect bodies of swimmers? Well, now you know how they get those types of bodies, seeing as swimming is a total body workout that really does help a huge number of different muscle groups.\nWhat's more, it is a form of resistance training too, so it serves as a strength building exercise as well.\n\n2.	Rowing\nJust like swimming, rowing is going to work just as many muscle groups, and also is a form of resistance training and so it shares similar benefits. All in all, this is a great way to work out, although it might be difficult if you don't have the necessary equipment (and lake!).\n\n3.	Aerobics\nDefinitely the most accessible of the three, aerobics by nature is geared towards providing as complete a body workout as possible, and it is something that you can even do from the comfort of your own home!\n\n\nIf you like swimming, great! If you have the necessary prerequisites for rowing, that's great too! But otherwise, aerobics is probably going to be the single-best option that you have in terms of getting in a total body workout.\n\nAnd now that you realize just how important these workouts can be, that's definitely what you want, right?",
                     group1) { CreatedOn = "Group", CreatedTxt = "The Ways and Tips", CreatedOnTwo = "Item", CreatedTxtTwo = "Total Body Exercises", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/02.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                                "Top 10 Insane Weight Loss Tips and Tricks",
                                "Want to lose weight fast? Tired of getting all the wrong information from all the wrong sources? Well, if you are keep reading because we're going to be going over the top 10 insane weight loss tips and tricks to help you get the most out of your efforts.",
                                "Assets/DarkGray.png",
                                "",
                                "Details:\n\nMost people gradually despair and start to feel that they'll never lose weight simply because they've been given the wrong kind of facts, or are simply on an approach that is never going to work. These tips will help solve problems like that, and provide you with an invaluable base from which to work from, so let's get started:Most people gradually despair and start to feel that they'll never lose weight simply because they've been given the wrong kind of facts, or are simply on an approach that is never going to work. These tips will help solve problems like that, and provide you with an invaluable base from which to work from, so let's get started:Most people gradually despair and start to feel that they'll never lose weight simply because they've been given the wrong kind of facts, or are simply on an approach that is never going to work. These tips will help solve problems like that, and provide you with an invaluable base from which to work from, so let's get started:\n\n1.	Eating more meals per day is good for you. But each of those meals should be smaller than a 'regular' meal. By consuming 5 'small' meals a day as opposed to 3 'big' ones, you're more likely to feel full and thus not overeat. Also, you're going to give your body an easier time of absorbing the energy that you gain from the food, and thus not store it as fat!\n\n2.	Try to plan your meals ahead as much as possible. That way you won't risk being caught out and having no choice but to eat some notoriously bad and unhealthy meals that just pack on the calories.\n\n3.	Drink water as often as possible, as it will help you to stay hydrated and will also help ensure that you don't feel ravenously hungry and end up overeating.\n\n4.	Exercise as much and as often as you can. Even something as simple as climbing the stairs to work instead of taking the lift is better than nothing, but if you can, set aside 40 minutes a day for a quick workout.\n\n5.	Set your goals and stick to them. Make sure they're realistic goals, but challenging enough that they keep you motivated.\n\n6.	Try different types of 'healthy' food so that you don't get bored of eating the same types of greens constantly. Nowadays there are a lot of options out there, so be sure to take advantage of them.\n\n7.	Combine cardiovascular exercise with weight training so that you're better able to start toning up your body muscles, look great, and have a faster metabolism.\n\n8.	Don't ever, ever, go on crash diets that will just probably end up backfiring on you.\n\n9.	Eat as many fruits and vegetables as you want, but try to cut down on consuming too much fat. When eating meat, be sure to remove the skin if possible.\n\n10.	Avoid late night snacks or having 'supper'. If you eat before you sleep, your body won't get the chance to work off all the energy it gets and is more likely to end up storing it.\n\nAll that sounds easy enough doesn't it? Everything said and done, weight loss really can be as simple as following these 10 tips, and they're undoubtedly going to serve you well when you decide to put them to use.\n\nNothing could be easier!",
                                group1) { CreatedOn = "Group", CreatedTxt = "The Ways and Tips", CreatedOnTwo = "Item", CreatedTxtTwo = "Top 10 Insane", bgColour = "#0099CC", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/03.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                                "Easily Fit a Weight Loss Plan into Your Daily Routine",
                                "If ever there was a common problem faced by most working adults when it comes to losing weight it's this one: There simply aren't enough hours in the day to be able to devote enough time to it.",
                                "Assets/DarkGray.png",
                                "",
                                "Details:\n\nAfter all, your daily routine probably consists of waking up early, going to work, grabbing lunch at the most convenient place possible before going back to work, and then finally grabbing dinner somewhere before going home utterly exhausted from the day.\n\nEven if you do have a few hours left free after you go home, chances are you already have other commitments including family, that you need to attend to.\n\nNow if this sounds familiar, you're not alone. Countless other working adults are in the same shoes and it's no surprise that you're having trouble finding the time to fit in a weight loss plan into your busy schedule. But although it may be tough, it isn't impossible.\n\nHere's how you start:\n\n1.	Find a diet that you can stick to\nDon't try to go for any specific diet if you're not going to be able to pick and choose what you eat while you work. Instead, try some alternatives with the sole idea of cutting down on your average calorie intake.\nBring some fruits to work to snack on as and when you can, and then eat a smaller lunch if you have to go out. Also, drink lots of water throughout the day, so that your appetite remains under control. Simple enough, right?\nOf course this is just an example. End of the day, the idea is to cut on your calories, so as long as you can find something that works for you, go with it!\n\n2.	Choose between gym workouts or home workouts\nSometimes there's just no time to go to the gym, especially if it is located quite a ways away, and you're going to waste time traveling to it before you head home. So what you could do instead is just workout at home.\nAlternatively, you could wake up earlier and get in a quick workout at the gym before you start work.Really, the choice is yours, but the idea is to choose the most convenient option to fit into your lifestyle. If you really can't find any time, even just 20 minutes of some exercise or other before you go to sleep is better than nothing at all and shouldn't be too hard to squeeze in!\n\nNotice how fitting in a weight loss plan is all about finding what you can cope with and going with it? For working adults, that really is the key that they require.\n\nThere are a variety of reasons you might want to get Tattoo name designs. There are also many different designs and types of lettering you can choose from. It's important to take the time to find the right one for you, so you can com\n\nAfter all, being on the 'best' option is pointless if you quit after a week, right?",
                                group1) { CreatedOn = "Group", CreatedTxt = "The Ways and Tips", CreatedOnTwo = "Item", CreatedTxtTwo = "Easily Fit a Weight Loss Plan", bgColour = "#33CC00", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/04.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                                "Healthy Weight Loss 101: Finding Out How to Best Go About It",
                                "When practically anyone starts to think about losing weight, the very first port of call that they often resort to is the most famous one: Dieting.",
                                "Assets/DarkGray.png",
                                "",
                                "Details:\n\nIn reality, all that dieting really involves is cutting down on your food intake. Or at least, that is the simple view of it. Truth be told, there's more to effective dieting than simply cutting down on food, or worse, starving yourself, and if you want the best results you're going to need to know more.\n\nOtherwise, you may or may not lose weight, and chances are, you'll end up facing a slew of other health problems too.\n\nFor quick and healthy weight loss, you need to remember that your body requires food to survive for two main purposes, both of which are of crucial importance and neither of which can be ignored:\n\n1.	Energy from food is required by the body to go about its daily routin\n2.	Nutrients from food are required by the body for various other purposes and bodily functions\n\nShould you cut down on your food intake and not gain sufficient energy, you'll find yourself feeling lethargic. And should you not get enough nutrients, you'll also have various other problems with your health - some of which could even be very serious.\n\nSo in order to lose weight healthily, you don't just want to cut back entirely on your food. Instead, you want to choose exactly what types of food you should cut back on, and what types you don't.\n\nFor example, you can safely cut out chocolate sundaes from your daily diet, but you shouldn't cut out any leafy, green, vegetables.\n\nWhile you're doing this, you'll also find that you're able to really start losing weight more effectively too. By differentiating between what you 'can' and 'can't' cut down on, you should be eliminating the more unhealthy parts of your diet, i.e. the ones with the most calories.\n\nOnce you do cut down on your calorie intake itself, your body will then automatically start to work through its 'stores' of energy, i.e. fat, and thus you'll be able to lose weight convincingly while still eating healthily.\n\nTruth is that eating a balanced and healthy diet is really the best thing that you could do when it comes to weight loss, so be sure to look up all the different nutrients and be certain that you're getting enough of them.\n\nThen, try to keep your calorie count to a minimum, while still maintaining your nutrient intake.\n\nFrankly, this is the key to healthy weight loss - limiting your calories, but not risking your body's health while you do so. Of course, you can further enhance your results through other methods, such as exercise, but this healthy diet should always be at the core of your efforts.\n\nTrust us, if you follow this method of weight loss, you should end up not only losing weight, but feeling better and healthier all round too!",
                                group1) { CreatedOn = "Group", CreatedTxt = "The Ways and Tips", CreatedOnTwo = "Item", CreatedTxtTwo = "Finding Out How to Best Go About It", bgColour = "#FFCC33", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/05.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                                "Debunking the Top 4 Myths of Weight Loss",
                                "Part of the reason why many find losing weight to be an uphill battle is simply the fact that there is a lot of misleading information out there. How many times have you read a resource about how to lose weight only to find that it directly contradicts something that you just read a few minutes before that?",
                                "Assets/DarkGray.png",
                                "",
                                "Details:\n\nAll these pieces of misleading information end up causing a lot of misconceptions, and these in turn just serve to muddle the entire issue. So right here and right now, let's cut through all the unwanted and unneeded myths, so that you're better able to lose weight in the best way possible!\n\n1.	Weight Loss Needs to be Complicated\nContrary to popular belief, there is nothing complicated about weight loss. In fact, the only reason it is viewed as complicated is because there is a lot of misinformation out there that could make it appear to be so.\nReally, some of the simplest programs for weight loss often turn out to be the most effective, due to the fact that they have very few 'frills' and 'unneeded extras' to just complicate the issue while providing no clear cut benefits.\n\n2.  Crash Diets Are the Best Way to Lose Weight Fast\nNext up: Crash diets, or 'fad diets' as they're otherwise known. Every so often, people encounter the latest crash diet that is essentially a spin off of all the previous ones, with a couple of extras thrown in to make it unique.\nAlmost all of these don't work, and if they do, their results are often reversible and any weight that you lose will end up coming back, sometimes with a vengeance.\n\n3.	All Fat is Bad For You While it is true that fat contains more calories, and certain types of fat are more likely to be stored as, well, fat, this doesn't mean that you need to avoid them completely. In fact, fats are more likely to help you feel 'full' faster, and thus stop you from overeating. Only certain types of fats, i.e. trans fats, are really bad for you, and so avoiding all fat is definitely not something worth doing.\n\n4.	To Diet You Need to Measure Every Calorie Consumed and Burnt\nAgain, this is completely untrue. Dieting is about cutting calories, yes, but you don't need to be precise and account for every last calorie in the food you eat. Neither do you need to calculate exactly how many calories you burn.\nInstead, so long as you're eating a healthy and wholesome diet, while still keeping your calorie count as low as you can, you'll be fine.\n\nNow that you know that all these four myths are exactly that - myths, you should be able to see that weight loss really isn't about complicated diet programs or designer diet pills. All you need is some knowledge, and the will to see a simple program through to its finish and you should find that you're losing weight better than ever before!",
                                group1) { CreatedOn = "Group", CreatedTxt = "The Ways and Tips", CreatedOnTwo = "Item", CreatedTxtTwo = "Debunking the Top 4 Myths", bgColour = "#0090C0", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/06.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                                "Eating Habits Most Conducive To Great Weight Loss",
                                "Ever heard of the phrase, 'You are what you eat'? Without a doubt that is part true, and you should already know that your calorie intake is going to be inextricably tied to your weight loss efforts. By consuming less calories, you'll force your body to burn through fat - that much is common knowledge.",
                                "Assets/DarkGray.png",
                                "",
                                "Details:\n\nHowever losing weight isn't just all about what you eat, it is about how you eat as well!\n\nWhat the above statement means is essentially that your eating habits can play as big a role in your weight loss efforts as the actual content of your meals does. Many people don't realize this, but eating habits can form a powerful weight loss tool, if you channel them correctly.\n\nSo let's start off by learning a few of the best eating habits that you could foster:\n\n1.	Eating many regular but 'smaller' meals\nMost people eat 3 main meals a day that are spaced out about 4 to 5 hours apart. As such, by the time they're done with one meal and ready for the next, they're pretty ravenous and this can often lead to overeating.\nIf you were to instead eat more 'small' meals throughout the day, you'll find that you're not exposed to this risk, and at the same time will give your body more time to metabolize the food and use up the energy that it gains from it.\n\n2.	Consume lots of water\nWater is simply irreplaceable and you should be drinking at least a liter or two a day. More importantly though, for weight loss, consuming water is going to ensure that you don't end up having hunger pangs, and overeating when it comes to meal time.\n\n3.	Never eat late at night\nWherever possible, try to ensure that your very last meal is at least a whole 4 hours before you go to sleep. That should allow your body ample time to use up the energy gained from it, so that there isn't any excess energy to be stored as fat when you sleep.\n\n4.	Snack on fruits wherever possible Occasionally we all crave a snack in between mealtimes, but if you can avoid fattening chocolate bars and sweets and instead much on an apple or orange, you're going to find that you're going to be cutting down on your calories a lot.\n\nEach of these four habits will play an important role in your weight loss regime, if you start to foster them. Not only will you find that your calorie intake is reduced by cutting on overeating and high-calorie snacks, but you'll also find that your newfound eating patterns allow your body to work through whatever calories you do consume in a more efficient manner.\n\nEnd of the day, this will add up to a powerful edge in your efforts to lose weight, as you'll quite literally be helping your body to properly make use of the energy it gains from food.\n\nAnd that is something that is certainly worth acquiring these habits for!",
                                group1) { CreatedOn = "Group", CreatedTxt = "The Ways and Tips", CreatedOnTwo = "Item", CreatedTxtTwo = "Eating Habits Most Conducive", bgColour = "#9999FF", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/07.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                                "Finding the Keys to Weight Loss in Cardiovascular Exercise",
                                "Constantly people who want to lose weight are told that they should start exercising. That much is certainly true, but unfortunately most people with no prior experience with exercise routines will find that they have no idea of where they should start, or what they should be doing.",
                                "Assets/DarkGray.png",
                                "",
                                "Details:\n\nIn its own right, this can lead to many other problems, and could even leave you feeling discouraged at your lack of progress even when you have been trying to get started with various forms of exercise.\n\nWhile it is true that any exercise is better than no exercise, it is also true that for weight loss in particular, some types of exercises work a lot better than others. So instead of wasting your time on what doesn't work, how about you do what does: Cardiovascular exercises!\n\nTime and time again, cardiovascular exercises (or aerobic exercises, as they're otherwise known) have been shown to help tremendously with weight loss. Generally speaking, these types of exercises encompass anything that gets your heart rate up, and covers a wide variety of exercises from walking, jogging, and running right on to rowing, and even aerobics itself.\n\nSo as you can see, there is certainly no shortage of options.\n\nDepending on the shape that you're in prior to beginning, you might want to start off with something light, and then slowly build up the intensity of your workouts. If you've led a fairly sedentary lifestyle, this will definitely be the case, and you could start off with brisk walks before graduating on to jogging and then finally running.\n\nNo matter what type of exercise you pick, you should try to keep to 20 minute workouts anything between 5 to 7 days a week. Some may even recommend 40 minute workouts, but to start out with 20 minutes is certainly fine.\n\nDo remember to be regular about your workouts though. As you workout regularly, your fitness level will generally improve, but skipping sessions here and there will mean that this improvement will come a lot slower than it otherwise would.\n\nWhen you workout in this fashion, you end up burning a lot of calories, which basically means you're expending your body's energy and forcing it to tap into those fat stores of energy that it has. Therefore you're quite literally going to be burning through fat, which is the entire purpose of the exercise to begin with.\n\nIn time, you'll find that continuous and regular cardiovascular workouts can help you to achieve everything that you want to in terms of weight loss. Of course, it is going to be a bit of a battle to start out with, but eventually you'll find that you honestly feel better after workouts than you did before, and that will make you feel like working out more.\n\nWithout a doubt, the key to weight loss is cardiovascular exercise, and now that you know exactly what to do, all that remains is for you to go out and get started!",
                                group1) { CreatedOn = "Group", CreatedTxt = "The Ways and Tips", CreatedOnTwo = "Item", CreatedTxtTwo = "Finding the Keys", bgColour = "#FFCC33", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/08.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                                "Losing Weight Made Fast and Easy With Proper Goals",
                                "Getting started with weight loss is never the easiest thing in the world. Not only are there numerous challenges that you'll have to face, both psychologically and physically, but the fact of the matter is that there are also so many options around that it could be troublesome figuring out where to start.",
                                "Assets/DarkGray.png",
                                "",
                                "Details:\n\nTruth is: The best place to start is by deciding what exactly you desire.\n\nTo put it in a way that is easier for you to appreciate, when you first start out you're undoubtedly going to have a thousand different things to think about all at once. As such, what you need to do before thinking about anything else is really sit down and ask yourself what you're trying to achieve.\n\nThis would be your very first step towards developing some proper goals, but when you start out, you might only have a very rough idea of what you want. Definitely, you want to lose some weight, but how much? Do you have an idea in terms of weight itself, i.e. 30 pounds or would you rather go by waistline in inches?\n\nReally, your goal can be in almost any terms you desire it to be, but the important thing is that you try to put figures to it if possible. Reason being, if you do put figures to it, you'll have an easy way of measuring your progress, and determining how effective the methods you use are.\n\nApart from that one 'main' goal though, that is your ultimate target, it would also be helpful for you to set numerous other 'mini goals' along the way, and put a time period to each of them.\n\nFor example, if you wanted to lose 30 pounds within 6 months, you could set yourself the fairly undemanding target of losing 5 pounds per month. That way, you'll have something to work against each and every month, rather than simply one final target that could seem to be a long way off.\n\nBy doing this, you'll find that you're less likely to procrastinate, and more likely to achieve that one final goal too.\n\nOf course, the only real prerequisite that your goals need to fill is the fact that they should be challenging, but at the same time realistic. Goals that aren't challenging are likely to get you no where, as you'll constantly be putting them off knowing that you could achieve them with ease.\n\nSimilarly, goals that are unrealistic are going to be impossible for you to achieve and will likely just discourage you.\n\nIf you can find the balance between the two though, you'll have exactly the type of goals that will keep you motivated, as well as give you a rewarding sense of achievement when you complete them.\n\nEnd of the day, losing weight is definitely a battle, but by setting up goals the way we've outlined, it's going to be one that you're more likely to win!",
                                group1) { CreatedOn = "Created  on", CreatedTxt = "3/4/2012", CreatedOnTwo = "Target Date", CreatedTxtTwo = "Losing Weight Made Fast and Easy", bgColour = "#66CCFF", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/09.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                    "Discover Healthy and Tasty Foods that Can Help Burn Fat",
                    "Why is it that for some reason, when people advise you to 'eat healthy' that normally means that you're going to have to end up eating bland and tasteless food that really doesn't satisfy you at all?",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nHonestly, the above can describe so many 'healthy' diets that are out there, but the truth is that while they may be nutritious and even low on calories, they end up making eating a chore, and often the tasteless nature of these diets put people off more than anything else.\n\nAnd the truth is that healthy eating doesn't have to be about eating bland or tasteless food at all. What you really need is to ensure that you're obtaining all the nutrients that your body needs, but apart from that you can really eat a whole variety of tasty foods.\n\nHere's a selection of tasty and yet nutritious foods that you can choose from:\n\n1.	Raw nuts\nSome people view nuts as being bad for your health, but they really aren't, provided you eat the right type of nuts, i.e. the raw kind, that haven't had tons of salt or sugar added to them.\nHonestly, nuts are a great way of satiating your appetite, while at the same time obtaining some pretty vital nutrients that can help you to maintain a low calorie yet nutritious diet.\n\n2.	Fruits\nWithout a doubt, fruits should be a fixture of any type of diet, mostly because they're immensely low on calories, and tremendously high on fiber and other nutrients. On top of that there are some great and tasty fruit out there, so be sure to explore them.\nOne type of fruit in particular stands out, and these are any form of berries. Unlike some other fruit that have high sugar contents, berries actually have a higher carbohydrates to sugar ratio and therefore are a great source of energy too!\n\n3.	Eggs\nAs is well known, eggs are a valuable source of protein, and although you may have heard that egg yolk is bad for you - don't remove it.\nTruth is, egg yolk isn't bad for you at all, and in fact it is the place where most of the egg's nutrients are contained. More importantly, whole eggs will fill you up faster than most other food types, and so they're very valuable for appetite control.\n\nSure, none of the above foods will 'burn fat' directly - but let's face it, there's no such thing as foods that directly burn fats. What these foods will do however is help you to burn fat by ensuring that you get enough nutrients while minimizing your calorie intake at the same time.\n\nThis means that your body will work through its energy stores eventually (i.e. fat). Of course, giving the process a helping hand by exercising can't hurt either!\n\nTry out these three types of food and you'll quickly find that your weight starts to drop towards the goal that you so yearn for.",
                    group1) { CreatedOn = "Group", CreatedTxt = "The Ways and Tips", CreatedOnTwo = "Item", CreatedTxtTwo = "Discover Healthy and Tasty Foods", bgColour = "#99CC33", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/10.jpg")), CurrentStatus = "HBP Remedies" });

            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                   "Some Recommendations",
                   "Some Recommendations",
                   "Assets/20.jpg",
                   "Losing weight is never the easiest thing in the world to accomplish, and that certainly isn't helped by the fact that there are so many 'miracle weight loss' products out there that never seem to work. That said, if you're having problem losing weight, you definitely would want to get help somehow - but where should you go, and what should you choose?\n\nGetting the right information is key, and that's why we've gone out there and taken 3 of the best weight loss products from amongst all the others and reviewed them for you as thoroughly as possible. By going through these reviews, you can see how each of these products has a different approach that is proven to work, and then decide which style best suits what you're looking for!");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Fat Loss 4 Idiots",
                    "Out of all the many, many fat loss products out there, Fat Loss 4 Idiots is a clear class apart, and not just because it has a really catchy name. Instead, this one-of-a-kind offering is unique in a way that can be described in a single word: Personalization.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nOut of all the many, many fat loss products out there, Fat Loss 4 Idiots is a clear class apart, and not just because it has a really catchy name. Instead, this one-of-a-kind offering is unique in a way that can be described in a single word: Personalization.\n\nInstead of just offering up the fairly standard 'guide' format that most weight loss programs tend to consist of, Fat Loss 4 Idiots takes things a step further and actually will design your very own diet for you. And it won't be the low-carb or 'calorie counting' variety of diet that you've probably been on before either.\n\nBy allowing you to pick foods which you like, and then doing all the grunt work for you, Fat Loss 4 Idiots will help you to come up with your own meal menus that you then just have to stick to. Also, these menus will change every 11 days so you'll never get stuck having to eat the same thing over and over again until you're sick of it.\n\nComplementing the entire process is a Diet Handbook that basically details everything you need to know about how to lose weight while on this program, and explains the 'calorie shifting' theory that all its planned diets use. All the information is put forward clearly, concisely, and in a simple rule-by-rule format that makes it a piece of cake to follow.\n\nIn fact, some of the handbook is even devoted to busting some of the common myths about weight loss, so by the time you've gone over it, you'll know exactly what to do and what not to do.\n\nTo be honest, one downside that you will find is the fact that Fat Loss 4 Idiots requires you to plan your meals beforehand and home-cook them wherever possible. If you live a lifestyle where you're rarely able to eat at home, or don't have the time and know-how to be able to cook your own meals, then you might face some difficulty.\n\nThat said, on the whole the simplicity and personal touch that Fat Loss 4 Idiots puts on the table is something that makes it a hands down winner out of all the products out there. Because it'll allow you to still eat foods that you like, you're going to be that much more likely to stick to your diet as well.\n\nAll in all, what this means is that you're more likely to lose weight and succeed in achieving your goals by using this product. That tremendous advantage alone should be more than enough reason for you to want to find out more, so be sure to take it under careful advisement when you're coming to a decision!\n\n For More Details Go To:\nhttp://bit.ly/14KdwaF",
                    group2) { CreatedOn = "Group", CreatedTxt = "Some Recommendations", CreatedOnTwo = "Item", CreatedTxtTwo = "Fat Loss 4 Idiots", bgColour = "#9933CC", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/21.jpg")), CurrentStatus = "HBP Remedies" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                     "Burn the Fat Feed the Muscle",
                     "From the very first glance, the one thing that sets Burn the Fat apart from other weight loss guides is the fact that it doesn't claim to be an 'overnight miracle cure'. Rather, right from the start it is stated clearly that while it will teach you how to get into great shape, it will also take work and effort on your part to achieve.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nFrom the very first glance, the one thing that sets Burn the Fat apart from other weight loss guides is the fact that it doesn't claim to be an 'overnight miracle cure'. Rather, right from the start it is stated clearly that while it will teach you how to get into great shape, it will also take work and effort on your part to achieve.\n\nConsidering that this is a niche where all too often there are products that bandy about claims without living up to them, this kind of upfront honesty says a lot about the nature of the approach used.\n\nThroughout this guide, you'll be taken through a no-nonsense look at how you can start burning off all that excess fat and turning it into lean muscle instead. Best part is that these are actual techniques that professional bodybuilders (like the writer!) use, but they're all laid out in layman terms that are easy to follow.\n\nSo what you end up getting are real insights into weight loss that is permanent and long lasting.\n\nYet another strong point of this guide is its comprehensive nature. Quite literally everything is covered, from motivation tips and tricks to keep you going, right on to what you need to know about the true facts of eating right so that you can turn flab into hard muscle.\n\nEntire sections are dedicated to giving you the keys that you'll need to succeed, and in each section the information that you need is provided in a clean and precise fashion. For instance, instead of just roughly letting you know that protein is good for you, Burn the Fat actually delves deeper and lets you know why it is good, how you should regard it, and the optimum protein intakes that you'll need to skyrocket your weight loss.\n\nFollowing this guide may take effort, but when you see just how much detail is provided, and how well thought out every aspect of it is, you'll realize that this is the real deal. And even though hard work is plainly stated to be a necessity, when you start seeing the kind of results you can attain after just three weeks, you'll see why it is worth the nominal $39.95 investment.\n\nWhen compared against other products, there are really only two aspects where it falls short. Firstly, while basic results can be seen quite quickly, there is a caveat that attaining your full goals may take a number of months. Furthermore, it is not personalized at all, and therefore while there are some choices you can make to suit your needs, it might not be for you.\n\nStill, if you're looking for a down to earth, realistic and extremely reliable way to get into fantastic shape, then this guide can and will give you the answers you require!\n\n For More Details Go To:\nhttp://bit.ly/ZBO31i",
                     group2) { CreatedOn = "Group", CreatedTxt = "Some Recommendations", CreatedOnTwo = "Item", CreatedTxtTwo = "Burn the Fat", bgColour = "#FF33CC", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/22.jpg")), CurrentStatus = "HBP Remedies" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                      "Master Cleanse Secrets 10 Day Diet",
                      "The Master Cleanse Secrets method is, as its name implies, based on the extremely popular Master Cleanse diet that even some celebrities such as Beyonce have used to lose weight quickly and effectively.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nThe Master Cleanse Secrets method is, as its name implies, based on the extremely popular Master Cleanse diet that even some celebrities such as Beyonce have used to lose weight quickly and effectively.\n\nNowadays it is a pretty well known diet, but the problem is that the Master Cleanse by nature introduces a lot of radical changes to your body, because it is essentially going to be flushing years of built up waste and toxins. For most beginners, these changes can be pretty tough to cope with.\n\nThat's where the main advantage of the Master Cleanse Secret comes into play: It gives you every last bit of information about the Master Cleanse so that by the time you're done with it, you'll know how to prepare and carry out your own cleanse while minimizing some of the nastier side effects that many people end up suffering from.\n\nAlso, to make sure that you continue to be able to enjoy the benefits of your weight loss, this guide will even go on to let you in on some little known secrets about what you should be doing after you're done with your cleanse.\n\nEssentially, you'll be able to carry out a thorough cleanse of your digestive system, completely ridding your body of all waste material, and losing a lot of weight in the process.\n\nIf in the past you've looked at the Master Cleanse diet, you'll know that one of the main disadvantages of it is that its results tend to be temporary. Dramatic weight loss never lasts, but there are chapters in the Master Cleanse Secrets that will help you can ensure you don't end up just facing a complete reversal.\n\nBe warned though: The Master Cleanse diet is a rigorous routine and is very demanding. Without a doubt, it isn't for everyone, but with this guide in your hand, you may just be able to pull it off, provided you stick to the plan that is outlined.\n\nTo be perfectly honest, this is the only reason this guide isn't right at the top spot of our reviewed products, coupled with the fact that there's such stiff competition that it is edged out just a little, but at $27 for just the guide itself (or $37 for the guide plus 3 other great complementing guides), it really is a sweet deal.\n\n For More Details Go To:\nhttp://bit.ly/15GbgDE",
                      group2) { CreatedOn = "Group", CreatedTxt = "Some Recommendations", CreatedOnTwo = "Item", CreatedTxtTwo = "Master Cleanse Secrets", bgColour = "#FF6600", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/23.jpg")), CurrentStatus = "HBP Remedies" });
            this.AllGroups.Add(group2);
			
           
        }
    }
}
