// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020 by daxnet. All rights reserved.
// Licensed under LGPL-v3
// ==============================================================

using System;

namespace Abacuza.Common.Models
{
    public sealed class S3File
    {
        public S3File()
        { }

        public S3File(string bucket, string key, string file)
        {
            Bucket = bucket;
            Key = key;
            File = file;
        }

        public string Bucket { get; set; }

        public string Key { get; set; }

        public string File { get; set; }

        public override bool Equals(object obj)
        {
            return obj is S3File file &&
                   Bucket == file.Bucket &&
                   Key == file.Key &&
                   File == file.File;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Bucket, Key, File);
        }

        public override string ToString() => $"s3a://{Bucket}/{Key}/{File}";


    }
}
