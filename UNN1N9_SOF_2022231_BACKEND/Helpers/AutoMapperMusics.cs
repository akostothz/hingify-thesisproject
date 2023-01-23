﻿using AutoMapper;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Helpers
{
    public class AutoMapperMusics : Profile
    {
        public AutoMapperMusics()
        {
            CreateMap<Music, MusicDto>();
        }
    }
}