﻿using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Buildings;
using StardewValley.Locations;
using System;
using System.Collections.Generic;

namespace CJBAutomation {
    public class CJBAutomation : Mod {

        public static ModConfig config;

        public override void Entry(params object[] objects) {
            config = new ModConfig().InitializeConfig(BaseConfigPath);

            TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;
        }

        private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e) {

            List<GameLocation> locations = new List<GameLocation>();

            foreach (GameLocation gLoc in Game1.locations) {

                if (!gLoc.name.Contains("Farm") && !gLoc.name.Contains("GreenHouse"))
                    continue;

                locations.Add(gLoc);

                if (gLoc is BuildableGameLocation) {
                    BuildableGameLocation bLoc = (BuildableGameLocation)gLoc;
                    foreach (Building build in bLoc.buildings) {
                        if (build.indoors != null)
                            locations.Add(build.indoors);
                    }
                }
            }

            foreach (GameLocation loc in locations) {
                foreach (KeyValuePair<Vector2, StardewValley.Object> kp in loc.objects) {
                    if (kp.Value == null)
                        continue;
                    ProcessObject(loc, kp.Key, kp.Value);
                }
            }
            locations.Clear();
        }

        private void ProcessObject(GameLocation gLoc, Vector2 objLoc, StardewValley.Object obj) {
            if (obj.name.Equals("Furnace")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    if (Automation.DoesChestsHaveItem(chests, 382, 1)) {
                        if (Automation.DoesChestsHaveItem(chests, 378, 5)) {
                            Automation.RemoveItemFromChests(chests, 382, 1);
                            Automation.RemoveItemFromChests(chests, 378, 5);
                            obj.heldObject = new StardewValley.Object(Vector2.Zero, 334, 1);
                            obj.minutesUntilReady = 30;
                        } else if (Automation.DoesChestsHaveItem(chests, 380, 5)) {
                            Automation.RemoveItemFromChests(chests, 382, 1);
                            Automation.RemoveItemFromChests(chests, 380, 5);
                            obj.heldObject = new StardewValley.Object(Vector2.Zero, 335, 1);
                            obj.minutesUntilReady = 120;
                        } else if (Automation.DoesChestsHaveItem(chests, 384, 5)) {
                            Automation.RemoveItemFromChests(chests, 382, 1);
                            Automation.RemoveItemFromChests(chests, 384, 5);
                            obj.heldObject = new StardewValley.Object(Vector2.Zero, 336, 1);
                            obj.minutesUntilReady = 300;
                        } else if (Automation.DoesChestsHaveItem(chests, 386, 5)) {
                            Automation.RemoveItemFromChests(chests, 382, 1);
                            Automation.RemoveItemFromChests(chests, 386, 5);
                            obj.heldObject = new StardewValley.Object(Vector2.Zero, 337, 1);
                            obj.minutesUntilReady = 480;
                        }
                    }
                    if (obj.heldObject == null && Automation.DoesChestsHaveItem(chests, 80, 1)) {
                        Automation.RemoveItemFromChests(chests, 80, 1);
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 338, "Refined Quartz", false, true, false, false);
                        obj.minutesUntilReady = 90;
                    }

                    if (obj.heldObject != null) {
                        obj.initializeLightSource(objLoc);
                        obj.showNextIndex = true;
                    }
                }
            } else if (obj.name.Equals("Crystalarium")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject.getOne()) == null) {
                                obj.minutesUntilReady = Automation.getMinutesForCrystalarium(obj.heldObject.parentSheetIndex);
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
            } else if (obj.name.Equals("Mayonnaise Machine")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    if (Automation.RemoveItemFromChests(chests, 176) || Automation.RemoveItemFromChests(chests, 180)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 306, (string)null, false, true, false, false);
                        obj.minutesUntilReady = 180;
                    } else
                    if (Automation.RemoveItemFromChests(chests, 107) || Automation.RemoveItemFromChests(chests, 174) || Automation.RemoveItemFromChests(chests, 182)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 306, (string)null, false, true, false, false) {
                            quality = 2
                        };
                        obj.minutesUntilReady = 180;
                    } else
                    if (Automation.RemoveItemFromChests(chests, 442)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 307, (string)null, false, true, false, false);
                        obj.minutesUntilReady = 180;
                    }
                }
            } else if (obj.name.Equals("Keg")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    if (Automation.RemoveItemFromChestsByName(chests, "Wheat", -1)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 346, "Beer", false, true, false, false);
                        obj.heldObject.name = "Beer";
                        obj.minutesUntilReady = 1750;
                    } else
                    if (Automation.RemoveItemFromChestsByName(chests, "Hops", -1)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 303, "Pale Ale", false, true, false, false);
                        obj.heldObject.name = "Pale Ale";
                        obj.minutesUntilReady = 2250;
                    } else {
                        StardewValley.Object item = (StardewValley.Object)Automation.GetItemFromChestsByCategory(chests, -79, -1);
                        if (item == null)
                            item = (StardewValley.Object)Automation.GetItemFromChestsByCategory(chests, -75, -1);

                        if (item != null) {
                            if (item.category == -79) {
                                obj.heldObject = new StardewValley.Object(Vector2.Zero, 348, item.Name + " Wine", false, true, false, false);
                                obj.heldObject.Price = item.Price * 3;
                                obj.heldObject.Name = item.Name + " Wine";
                                obj.minutesUntilReady = 10000;
                                Automation.RemoveItemFromChestsCategory(chests, -79, -1);
                            }
                            if (item.category == -75) {
                                obj.heldObject = new StardewValley.Object(Vector2.Zero, 350, item.Name + " Juice", false, true, false, false);
                                obj.heldObject.Price = (int)(item.price * 2.25d);
                                obj.heldObject.Name = item.Name + " Juice";
                                obj.minutesUntilReady = 6000;
                                Automation.RemoveItemFromChestsCategory(chests, -75, -1);
                            }
                        }
                    }
                }
            } else if (obj.name.Equals("Charcoal Kiln")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    if (Automation.RemoveItemFromChests(chests, 388, 10)) {
                        obj.heldObject = new StardewValley.Object(382, 1, false, -1, 0);
                        obj.minutesUntilReady = 30;
                        obj.showNextIndex = true;
                    }
                }
            } else if (obj.name.Equals("Cheese Press")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    if (Automation.RemoveItemFromChests(chests, 184)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 424, null, false, true, false, false);
                        obj.minutesUntilReady = 200;
                    } else if (Automation.RemoveItemFromChests(chests, 186)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 424, "Cheese (=)", false, true, false, false) {
                            quality = 2
                        };
                        obj.minutesUntilReady = 200;
                    } else if (Automation.RemoveItemFromChests(chests, 436)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 426, null, false, true, false, false);
                        obj.minutesUntilReady = 200;
                    } else if (Automation.RemoveItemFromChests(chests, 438)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 426, null, false, true, false, false) {
                            quality = 2
                        };
                        obj.minutesUntilReady = 200;
                    }
                }
            } else if (obj.name.Equals("Preserves Jar")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    StardewValley.Object item = (StardewValley.Object)Automation.GetItemFromChestsByCategory(chests, -79, -1);
                    if (item == null)
                        item = (StardewValley.Object)Automation.GetItemFromChestsByCategory(chests, -75, -1);

                    if (item != null) {
                        if (item.category == -79) {
                            obj.heldObject = new StardewValley.Object(Vector2.Zero, 344, item.Name + " Jelly", false, true, false, false);
                            obj.heldObject.Price = 50 + item.Price * 2;
                            obj.heldObject.Name = item.Name + " Jelly";
                            obj.minutesUntilReady = 4000;
                            Automation.RemoveItemFromChestsCategory(chests, -79, -1);
                        }
                        if (item.category == -75) {
                            obj.heldObject = new StardewValley.Object(Vector2.Zero, 342, "Pickled " + item.Name, false, true, false, false);
                            obj.heldObject.Price = 50 + item.Price * 2;
                            obj.heldObject.Name = "Pickled " + item.Name;
                            obj.minutesUntilReady = 4000;
                            Automation.RemoveItemFromChestsCategory(chests, -75, -1);
                        }
                    }
                }
            } else if (obj.name.Equals("Loom")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                obj.showNextIndex = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    if (Automation.RemoveItemFromChests(chests, 440)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 428, null, false, true, false, false);
                        obj.minutesUntilReady = 240;
                        obj.showNextIndex = true;
                    }
                }
            } else if (obj.name.Equals("Bee House") && !Game1.currentSeason.Equals("winter")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.items.Count < 36) {
                                string str = "Wild";
                                int price = 0;
                                if (gLoc is Farm) {
                                    Crop crop = Utility.findCloseFlower(obj.tileLocation);
                                    if (crop != null) {
                                        str = Game1.objectInformation[crop.indexOfHarvest].Split(new char[] { '/' })[0];
                                        price = Convert.ToInt32(Game1.objectInformation[crop.indexOfHarvest].Split(new char[] { '/' })[1]) * 2;
                                    }
                                }
                                obj.heldObject.name = str + " Honey";
                                obj.heldObject.price += price;
                                if (chest.addItem(obj.heldObject) == null) {
                                    obj.heldObject = new StardewValley.Object(Vector2.Zero, 340, null, false, true, false, false);
                                    if (Game1.currentSeason.Equals("winter")) {
                                        obj.heldObject = null;
                                    }
                                    obj.minutesUntilReady = 2400 - Game1.timeOfDay + 4320;
                                    obj.readyForHarvest = false;
                                    obj.showNextIndex = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            } else if (obj.name.Equals("Worm Bin")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = new StardewValley.Object(685, Game1.random.Next(2, 6), false, -1, 0);
                                obj.minutesUntilReady = 2400 - Game1.timeOfDay;
                                obj.readyForHarvest = false;
                                obj.showNextIndex = false;
                                break;
                            }
                        }
                    }
                }
            } else if (obj.name.Equals("Seed Maker")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    int seedId = Automation.RemoveItemFromChestsIfCrop(chests);
                    if (seedId != -1) {
                        Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)((int)obj.tileLocation.X) + (uint)((int)obj.tileLocation.Y * 77) + (uint)Game1.timeOfDay));
                        obj.heldObject = new StardewValley.Object(seedId, random.Next(1, 4), false, -1, 0);
                        if (random.NextDouble() < 0.005) {
                            obj.heldObject = new StardewValley.Object(499, 1, false, -1, 0);
                        } else if (random.NextDouble() < 0.02) {
                            obj.heldObject = new StardewValley.Object(770, random.Next(1, 5), false, -1, 0);
                        }
                        obj.minutesUntilReady = 20;
                    }
                }
            } else if (obj.name.Equals("Recycling Machine")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    Random random2 = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + Game1.timeOfDay + (int)obj.tileLocation.X * 200 + (int)obj.tileLocation.Y);
                    if (Automation.RemoveItemFromChests(chests, 168)) {
                        obj.heldObject = new StardewValley.Object((random2.NextDouble() < 0.3) ? 382 : ((random2.NextDouble() < 0.3) ? 380 : 390), random2.Next(1, 4), false, -1, 0);
                        obj.minutesUntilReady = 60;
                        Game1.stats.PiecesOfTrashRecycled += 1u;
                    } else if (Automation.RemoveItemFromChests(chests, 169)) {
                        obj.heldObject = new StardewValley.Object((random2.NextDouble() < 0.25) ? 382 : 388, random2.Next(1, 4), false, -1, 0);
                        obj.minutesUntilReady = 60;
                        Game1.stats.PiecesOfTrashRecycled += 1u;
                    } else if (Automation.RemoveItemFromChests(chests, 170) || Automation.RemoveItemFromChests(chests, 171)) {
                        obj.heldObject = new StardewValley.Object(338, 1, false, -1, 0);
                        obj.minutesUntilReady = 60;
                        Game1.stats.PiecesOfTrashRecycled += 1u;
                    } else if (Automation.RemoveItemFromChests(chests, 172)) {
                        obj.heldObject = ((random2.NextDouble() < 0.1) ? new StardewValley.Object(428, 1, false, -1, 0) : new Torch(Vector2.Zero, 3));
                        obj.minutesUntilReady = 60;
                        Game1.stats.PiecesOfTrashRecycled += 1u;
                    }
                }
            } else if (obj.name.Equals("Oil Maker")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
                if (obj.heldObject == null && !obj.readyForHarvest) {
                    if (Automation.RemoveItemFromChests(chests, 270)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 247, null, false, true, false, false);
                        obj.minutesUntilReady = 1000;
                    } else if (Automation.RemoveItemFromChests(chests, 421)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 247, null, false, true, false, false);
                        obj.minutesUntilReady = 60;
                    } else if (Automation.RemoveItemFromChests(chests, 430)) {
                        obj.heldObject = new StardewValley.Object(Vector2.Zero, 432, null, false, true, false, false);
                        obj.minutesUntilReady = 360;
                    } else if (Automation.RemoveItemFromChests(chests, 431)) {
                        obj.heldObject = new StardewValley.Object(247, 1, false, -1, 0);
                        obj.minutesUntilReady = 3200;
                    }
                }
            } else if (obj.name.Equals("Tapper")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject) == null) {

                                int id = obj.heldObject.parentSheetIndex;

                                if (id == 724) {
                                    obj.minutesUntilReady = 16000 - Game1.timeOfDay;
                                } else if (id == 725) {
                                    obj.minutesUntilReady = 13000 - Game1.timeOfDay;
                                } else if (id == 726) {
                                    obj.minutesUntilReady = 10000 - Game1.timeOfDay;
                                } else if (id == 422) {
                                    obj.minutesUntilReady = 3000 - Game1.timeOfDay;
                                    obj.heldObject = new StardewValley.Object(420, 1, false, -1, 0);
                                } else if (id == 404 || id == 420) {
                                    obj.minutesUntilReady = 3000 - Game1.timeOfDay;
                                    if (!Game1.currentSeason.Equals("fall")) {
                                        obj.heldObject = new StardewValley.Object(404, 1, false, -1, 0);
                                        obj.minutesUntilReady = 6000 - Game1.timeOfDay;
                                    }
                                    if (Game1.dayOfMonth % 10 == 0) {
                                        obj.heldObject = new StardewValley.Object(422, 1, false, -1, 0);
                                    }
                                    if (Game1.currentSeason.Equals("winter")) {
                                        obj.minutesUntilReady = 80000 - Game1.timeOfDay;
                                    }
                                }
                                if (obj.heldObject != null) {
                                    obj.heldObject = (StardewValley.Object)obj.heldObject.getOne();
                                }
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
            } else if (obj.name.Equals("Lightning Rod")) {
                List<Chest> chests = Automation.GetChestsFromSurroundingLocation(gLoc, objLoc);
                if (obj.heldObject != null && obj.readyForHarvest) {
                    foreach (Chest chest in chests) {
                        if (chest != null) {
                            if (chest.addItem(obj.heldObject.getOne()) == null) {
                                obj.heldObject = null;
                                obj.readyForHarvest = false;
                                break;
                            }
                        }
                    }
                }
            }
            // end
        }
    }


    public class ModConfig : Config {

        public bool diagonal { get; set; }

        public override T GenerateDefaultConfig<T>() {

            diagonal = false;

            return this as T;
        }

    }
}
